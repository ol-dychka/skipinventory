from pathlib import Path

from fastapi import APIRouter, HTTPException

from models.linear import RidgeRegressionGD
from schemas.prediction_payload import PredictionPayload
from pipeline.features import build_features
from pipeline.order_quantity import calculate_order_quantity
from pipeline.preprocessor import Preprocessor

BASE_DIR = Path(__file__).resolve().parent.parent.parent
WEIGHTS_PATH = BASE_DIR / "artifacts" / "weights.npy"
PREPROCESSING_PATH = BASE_DIR / "artifacts" / "preprocessing.npy"

router = APIRouter(prefix="/predict", tags=["predictions"])
model = RidgeRegressionGD()
preprocessor = Preprocessor()

try:    
    model.load(WEIGHTS_PATH)
    preprocessor.load(PREPROCESSING_PATH)
except FileNotFoundError:
    pass

@router.post("/single")
def predict_single(payload: PredictionPayload) -> dict:
    if model.weights is None:
        raise HTTPException(status_code=503, detail="Model not trained yet")
    
    features = build_features(payload.sales, payload.product)
    features_scaled = preprocessor.transform(features.reshape(1, -1))
    predicted_demand = float(model.predict(features_scaled)[0])
    demand_std = features[3]

    order_quantity = calculate_order_quantity(predicted_demand, payload.product, demand_std)

    return {
        "sku": payload.product.sku,
        "id": payload.product.id,
        "predicted_weekly_demand": round(predicted_demand, 2),
        "recommended_order_quantity": order_quantity
    }

@router.post("/batch")
def predict_batch(payload: list[PredictionPayload]):
    try:
        if model.weights is None:
            raise HTTPException(status_code=503, detail="Model not trained yet")
        
        result = []
        for item in payload: 
            features = build_features(item.sales, item.product)
            features_scaled = preprocessor.transform(features.reshape(1, -1))
            predicted_demand = float(model.predict(features_scaled)[0])
            demand_std = features[3]

            order_quantity = calculate_order_quantity(predicted_demand, item.product, demand_std)

            result.append({
                "sku": item.product.sku,
                "id": item.product.id,
                "predicted_weekly_demand": round(predicted_demand, 2),
                "recommended_order_quantity": order_quantity
            })

        return result
    except Exception as e:
        import traceback
        print("ENDPOINT ERROR:", traceback.format_exc())
        raise