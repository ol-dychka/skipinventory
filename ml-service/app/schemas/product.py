from datetime import date
from typing import Optional
from pydantic import BaseModel

class Product (BaseModel):
    id: str
    name: str
    sku: str
    category: Optional[str]
    vendor: str
    unit: str

    cost_price: float
    sale_price: float
    currency: str

    current_stock: int
    reorder_point: int
    base_reorder_quantity: int
    delivery_delay: int

    is_active: bool
    created_at: date
    updated_at: date

    organization_id: str

