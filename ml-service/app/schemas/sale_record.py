from datetime import date
from pydantic import BaseModel

class SaleRecord (BaseModel):
    id: str
    sku: str
    date: date

    units_sold: int
    units_returned: int

    organization_id: str
    product_id: str
