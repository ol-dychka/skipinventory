from pathlib import Path
from fastapi import APIRouter
from app.scripts.generate_synthetic_data import write
from app.pipeline.trainer import train

router = APIRouter(prefix="/train", tags=["training"])

BASE_DIR = Path(__file__).resolve().parent.parent.parent
DATA_PATH = BASE_DIR / "data" / "training_data.json"
WEIGHTS_PATH = BASE_DIR / "artifacts" / "weights.npy"
PREPROCESSING_PATH = BASE_DIR / "artifacts" / "preprocessing.npy"

@router.post("/generate")
def generate_training_data():
    write()

    return

@router.post("/data")
def train_on_generated_data():
    train(DATA_PATH, WEIGHTS_PATH, PREPROCESSING_PATH)

    return