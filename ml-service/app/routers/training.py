import os

from fastapi import APIRouter, BackgroundTasks, HTTPException

from models.linear import RidgeRegressionGD
from schemas.prediction_payload import PredictionPayload
from pipeline.features import build_features
from forecasting.reorder import calculate_order_quantity
from scripts.generate_synthetic_data import write

router = APIRouter(prefix="/train", tags=["training"])

training_state = {
    "status": "idle",       # idle | running | done | failed
    "metrics": None,
    "error": None,
}

@router.post("/generate")
def generate_training_data():
    write()

    return

@router.post("/file/async")
def train_from_file_async(req: TrainFromFileRequest, background_tasks: BackgroundTasks):
    """
    Non-blocking version — returns immediately, trains in background.
    Poll /train/status to check progress.
    Use this if training takes more than a few seconds.
    """
    if training_state["status"] == "running":
        raise HTTPException(status_code=409, detail="Training already in progress")

    if not os.path.exists("data/training_data.json"):
        raise HTTPException(status_code=404, detail=f"File not found: {req.data_path}")

    def _task():
        training_state.update({"status": "running", "metrics": None, "error": None})
        try:
            X, y = load_training_data("data/training_data.json")
            metrics = run_training(X, y, 0.01, 2000, 0.01)
            training_state.update({"status": "done", "metrics": metrics.model_dump()})
        except Exception as e:
            training_state.update({"status": "failed", "error": str(e)})

    background_tasks.add_task(_task)
    return {"status": "started", "message": "Poll /train/status for updates"}


@router.get("/status")
def training_status():
    """Check the result of an async training run."""
    return training_state