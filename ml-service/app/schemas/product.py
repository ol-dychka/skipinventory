from datetime import date, datetime
from typing import Optional
from pydantic import BaseModel, ConfigDict, Field

class Product (BaseModel):
    model_config = ConfigDict(populate_by_name=True)
    
    id: str = Field(alias="Id")
    name: str = Field(alias="Name")
    sku: str = Field(alias="Sku")
    category: Optional[str] = Field(alias="Category")
    vendor: str = Field(alias="Vendor")
    unit: str = Field(alias="Unit")
    organization_id: str = Field(alias="OrganizationId")

    cost_price: float = Field(alias="CostPrice")
    sale_price: float = Field(alias="SalePrice")

    current_stock: int = Field(alias="CurrentStock")
    reorder_point: int = Field(alias="ReorderPoint")
    base_reorder_quantity: int = Field(alias="BaseReorderQuantity")
    delivery_delay: int = Field(alias="DeliveryDelay")

