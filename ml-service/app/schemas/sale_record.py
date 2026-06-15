from datetime import datetime
from pydantic import BaseModel, ConfigDict, Field

class SaleRecord (BaseModel):
    model_config = ConfigDict(populate_by_name=True)

    id: str = Field(alias="Id")
    sku: str = Field(alias="Sku")
    date: datetime = Field(alias="Date")

    units_sold: int = Field(alias="UnitsSold")
    units_returned: int = Field(alias="UnitsReturned")

    organization_id: str = Field(alias="OrganizationId")
    product_id: str = Field(alias="ProductId")
