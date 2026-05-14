from pathlib import Path

from fastapi import APIRouter, HTTPException

from models.linear import RidgeRegressionGD
from schemas.prediction_payload import PredictionPayload
from pipeline.features import build_features
from pipeline.order_quantity import calculate_order_quantity

BASE_DIR = Path(__file__).resolve().parent.parent.parent
WEIGHTS_PATH = BASE_DIR / "aftifacts" / "weights.json"

router = APIRouter(prefix="/predict", tags=["predictions"])
model = RidgeRegressionGD()

try:    
    model.load(WEIGHTS_PATH)
except FileNotFoundError:
    pass

@router.post("/single")
def predict_single(payload: PredictionPayload) -> dict:
    if model.weights is None:
        raise HTTPException(status_code=503, detail="Model not trained yet")
    
    features = build_features(payload.sales, payload.product)
    predicted_demand = float(model.predict(features.reshape(1, -1))[0])
    demand_std = features[3]

    order_quantity = calculate_order_quantity(predicted_demand, payload.product, demand_std)

    return {
        "sku": payload.product.sku,
        "predicted_weekly_demand": round(predicted_demand, 2),
        "recommended_order_quantity": order_quantity
    }

@router.post("/batch")
def predict_batch(payload: list[PredictionPayload]):
    if model.weights is None:
        raise HTTPException(status_code=503, detail="Model not trained yet")
    
    result = []
    for item in payload: 
        features = build_features(item.sales, item.product)
        predicted_demand = float(model.predict(features.reshape(1, -1))[0])
        demand_std = features[3]

        order_quantity = calculate_order_quantity(predicted_demand, item.product, demand_std)

        result.append({
            "sku": item.product.sku,
            "predicted_weekly_demand": round(predicted_demand, 2),
            "recommended_order_quantity": order_quantity
        })

    return result