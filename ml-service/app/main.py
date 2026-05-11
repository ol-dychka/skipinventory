from fastapi import FastAPI
from models.linear import RidgeRegressionGD
from schemas.product import Product
from schemas.sale_record import SaleRecord
from pipeline.features import build_features
from forecasting.reorder import calculate_order_quantity

app = FastAPI()
model = RidgeRegressionGD()
model.load("artifacts/weights.npy")

@app.post("/predict/reorder-quantity")
def predict_reorder_quantity(sales: list[SaleRecord], product: Product) -> dict:
    features = build_features(sales, product)
    X = features.reshape(1, -1)
    predicted_demand = float(model.predict(X)[0])
    demand_std = features[3]

    order_quantity = calculate_order_quantity(predicted_demand, product, demand_std)

    return {
        "sku": product.sku,
        "predicted_weekly_demand": round(predicted_demand, 2),
        "recommended_order_quantity": order_quantity
    }