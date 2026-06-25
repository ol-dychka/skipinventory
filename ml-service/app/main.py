from fastapi import FastAPI
from app.routers.predictions import router as predictionsRouter
from app.routers.training import router as trainingRouter
from fastapi import Request
from fastapi.responses import JSONResponse

app = FastAPI()
app.include_router(predictionsRouter)
app.include_router(trainingRouter)

@app.exception_handler(Exception)
async def general_exception_handler(request: Request, exc: Exception):
    import traceback
    print("UNHANDLED EXCEPTION:", traceback.format_exc())
    return JSONResponse(status_code=500, content={"detail": str(exc)})
