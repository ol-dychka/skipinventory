from fastapi import FastAPI
from routers.predictions import router

app = FastAPI()
app.include_router(router)
