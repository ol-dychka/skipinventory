import numpy as np

class Preprocessor:
    def __init__(self):
        self.mean = None
        self.std = None

    def fit(self, X: np.ndarray):
        self.mean = X.mean(axis=0)
        self.std = X.std(axis=0)
        self.std[self.std == 0] = 1
        return self

    def transform(self, X: np.ndarray) -> np.ndarray:
        if self.mean is None or self.std is None:
            raise RuntimeError("Preprocessor must be fit before transform")
        return (X - self.mean) / self.std

    def fit_transform(self, X: np.ndarray) -> np.ndarray:
        return self.fit(X).transform(X)

    def save(self, path: str):
        np.save(path, {"mean": self.mean, "std": self.std})

    def load(self, path: str):
        data = np.load(path, allow_pickle=True).item()
        self.mean = data["mean"]
        self.std = data["std"]