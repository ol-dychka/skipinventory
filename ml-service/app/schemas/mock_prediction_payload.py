from pydantic import BaseModel
from schemas.sale_record import SaleRecord
from schemas.product import Product


class MockPredictionPayload (BaseModel):
    sales: list[SaleRecord]
    product: Product
    orderQuantity: int