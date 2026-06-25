from pydantic import BaseModel
from app.schemas.sale_record import SaleRecord
from app.schemas.product import Product


class MockPredictionPayload (BaseModel):
    sales: list[SaleRecord]
    product: Product
    orderQuantity: int