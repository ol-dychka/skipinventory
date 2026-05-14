import numpy as np

class Preprocessor:
    def __init__(self):
        self.mean = None
        self.std = None
        self.feature_names: list[str] = []

    def fit(self, X: np.ndarray, feature_names: list[str] = None):
        """Compute mean/std from training set only."""
        self.mean = X.mean(axis=0)
        self.std = X.std(axis=0)
        self.std[self.std == 0] = 1  # avoid divide-by-zero on constant features
        self.feature_names = feature_names or []

    def transform(self, X: np.ndarray) -> np.ndarray:
        """Apply stored normalization — call on both train and inference data."""
        if self.mean is None:
            raise RuntimeError("Preprocessor must be fit before transform")
        return (X - self.mean) / self.std

    def fit_transform(self, X: np.ndarray, feature_names: list[str] = None) -> np.ndarray:
        self.fit(X, feature_names)
        return self.transform(X)

    def save(self, path: str):
        np.save(path, {"mean": self.mean, "std": self.std, "features": self.feature_names})

    def load(self, path: str):
        data = np.load(path, allow_pickle=True).item()
        self.mean = data["mean"]
        self.std = data["std"]
        self.feature_names = data["features"]