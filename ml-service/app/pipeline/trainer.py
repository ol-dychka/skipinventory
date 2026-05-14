import numpy as np
import json
from app.schemas import SaleRecord, Product
from app.pipeline.features import build_features
from app.pipeline.preprocessor import Preprocessor
from app.models.linear import RidgeRegressionGD

def load_training_data(path: str) -> tuple[np.ndarray, np.ndarray]:
    """
    Expects data/training_data.json to be a list of:
    { "sales": [...], "product": {...}, "actual_order_quantity": 42 }
    """
    with open(path) as f:
        records = json.load(f)

    X_rows, y_rows = [], []
    for record in records:
        sales = [SaleRecord(**s) for s in record["sales"]]
        product = Product(**record["product"])
        features = build_features(sales, product)
        X_rows.append(features)
        y_rows.append(record["actual_order_quantity"])

    return np.array(X_rows), np.array(y_rows)

def train_evaluate(data_path="data/training_data.json"):
    X, y = load_training_data(data_path)

    # Train/test split (no sklearn — manual slice)
    split = int(len(X) * 0.8)
    X_train, X_test = X[:split], X[split:]
    y_train, y_test = y[:split], y[split:]

    preprocessor = Preprocessor()
    X_train_norm = preprocessor.fit_transform(X_train)
    X_test_norm = preprocessor.transform(X_test)

    model = RidgeRegressionGD()
    model.fit(X_train_norm, y_train)

    # Evaluation metrics — implemented manually
    y_pred = model.predict(X_test_norm)
    mae = np.abs(y_pred - y_test).mean()
    rmse = np.sqrt(((y_pred - y_test) ** 2).mean())
    ss_res = ((y_test - y_pred) ** 2).sum()
    ss_tot = ((y_test - y_test.mean()) ** 2).sum()
    r2 = 1 - ss_res / ss_tot

    print(f"MAE: {mae:.2f} | RMSE: {rmse:.2f} | R²: {r2:.4f}")

    model.save("artifacts/weights.npy")
    preprocessor.save("artifacts/preprocessor.npy")
    print("Artifacts saved.")

if __name__ == "__main__":
    train_evaluate()