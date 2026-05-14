import numpy as np
import json
from schemas.sale_record import SaleRecord
from schemas.product import Product
from pipeline.features import build_features
from models.linear import RidgeRegressionGD
from pipeline.preprocessor import Preprocessor

def load_data(path: str) -> tuple[np.ndarray, np.ndarray]:
    with open(path) as f:
        records = json.load(f)

    X_rows, y_rows = [], []
    for record in records:
        sales = [SaleRecord(**s) for s in record["sales"]]
        product = Product(**record["product"])
        features = build_features(sales, product)
        X_rows.append(features)
        y_rows.append(record["orderQuantity"])

    return np.array(X_rows), np.array(y_rows)

def train(data_path, weights_path, preprocessing_path):
    X, y = load_data(data_path)

    print("X shape:", X.shape)
    print("X NaNs:", np.isnan(X).sum())
    print("y NaNs:", np.isnan(y).sum())
    print("y sample:", y[:5])

    split = int(len(X) * 0.8)
    X_train, X_test = X[:split], X[split:]
    y_train, y_test = y[:split], y[split:]

    preprocessor = Preprocessor()
    X_train_scaled = preprocessor.fit_transform(X_train)
    X_test_scaled = preprocessor.transform(X_test)

    model = RidgeRegressionGD()
    model.fit(X_train_scaled, y_train)

    # Evaluation metrics — implemented manually
    y_pred = model.predict(X_test_scaled)
    mae = np.abs(y_pred - y_test).mean()
    rmse = np.sqrt(((y_pred - y_test) ** 2).mean())
    ss_res = ((y_test - y_pred) ** 2).sum()
    ss_tot = ((y_test - y_test.mean()) ** 2).sum()
    r2 = 1 - ss_res / ss_tot

    print(f"MAE: {mae:.2f} | RMSE: {rmse:.2f} | R²: {r2:.4f}")

    model.save(weights_path)
    preprocessor.save(preprocessing_path)
    print("Artifacts saved.")