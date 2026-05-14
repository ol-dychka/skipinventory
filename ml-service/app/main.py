from fastapi import FastAPI
from routers.predictions import router as predictionsRouter
from routers.training import router as trainingRouter

app = FastAPI()
app.include_router(predictionsRouter)
app.include_router(trainingRouter)
