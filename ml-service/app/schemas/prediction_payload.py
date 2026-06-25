from pydantic import BaseModel, ConfigDict, Field
from app.schemas.sale_record import SaleRecord
from app.schemas.product import Product


class PredictionPayload (BaseModel):
    model_config = ConfigDict(populate_by_name=True)

    sales: list[SaleRecord] = Field(alias="Sales")
    product: Product = Field(alias="Product")