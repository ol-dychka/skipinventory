import numpy as np

class RidgeRegressionGD:
    def __init__(self, lr=0.01, epochs=1000, lambda_=0.01):
        self.lr = lr
        self.epochs = epochs
        self.lamdba_ = lambda_
        self.weights = None
        self.bias = 0.0
        self.loss_history = []

    def fit(self, X: np.ndarray, y: np.ndarray):
        n, m = X.shape
        self.weights = np.zeros(m)

        for _ in range(self.epochs):
            y_pred = X @ self.weights + self.bias
            error = y_pred - y

            dw = (X.T @ error) / n + self.lambda_ * self.weights
            db = error.mean()

            self.weights -= self.lr * dw
            self.bias -= self.lr * db

            loss = (error ** 2).mean() + self.lamdba_ * (self.weights ** 2).sum()
            self.loss_history.append(loss)

    def predict(self, X:np.ndarray) -> np.ndarray:
        return X @ self.weights + self.bias
    
    def save(self, path: str):
        np.save(path, {"weights": self.weights, "bias": self.bias})

    def load(self, path: str):
        data = np.load(path, allow_pickle=True).item()
        self.weights, self.bias = data["weights"], data["bias"]
