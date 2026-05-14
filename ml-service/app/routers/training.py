import os
from pathlib import Path

from fastapi import APIRouter


from scripts.generate_synthetic_data import write
from pipeline.trainer import train

router = APIRouter(prefix="/train", tags=["training"])

BASE_DIR = Path(__file__).resolve().parent.parent.parent
DATA_PATH = BASE_DIR / "data" / "training_data.json"
WEIGHTS_PATH = BASE_DIR / "aftifacts" / "weights.npy"

training_state = {
    "status": "idle",       # idle | running | done | failed
    "metrics": None,
    "error": None,
}

@router.post("/generate")
def generate_training_data():
    write()

    return

@router.post("/data")
def train_on_generated_data():
    train()

    return