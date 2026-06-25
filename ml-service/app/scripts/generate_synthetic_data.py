import uuid

from faker import Faker
import numpy as np
import json
from datetime import date, timedelta
import random
from app.schemas.product import Product
from app.schemas.sale_record import SaleRecord
from app.schemas.mock_prediction_payload import MockPredictionPayload
from pathlib import Path

fake = Faker()

BASE_DIR = Path(__file__).resolve().parent.parent.parent
DATA_PATH = BASE_DIR / "data" / "training_data.json"

CATEGORIES = ["Electronics", "Clothing", "Food", "New", "Sale", None]
PATTERNS = ["stable", "seasonal", "trending_up", "erratic"]

def generate_product() -> Product:
    cost = round(random.uniform(1, 500), 2)
    created = fake.date_between(start_date="-3y", end_date="-1m")

    return Product(
        id = str(uuid.uuid4()),
        name = fake.catch_phrase(),
        sku = fake.bothify("SKU-####-???").upper(),
        category = random.choice(CATEGORIES),
        vendor=fake.company(),
        unit="Each",

        cost_price=cost,
        sale_price=round(cost * random.uniform(1.1, 2.5), 2),
        currency="CAD",

        current_stock = random.randint(0, 100),
        reorder_point = random.randint(10, 50),
        base_reorder_quantity = random.randint(10, 50),
        delivery_delay = random.randint(1, 20),

        is_active = random.choices([True, False], weights=[80, 20])[0],
        created_at = created,
        updated_at = fake.date_between(start_date=created, end_date="today"),

        organization_id = str(uuid.uuid4()),
    )

def generate_sale_records(
    product: Product,
    pattern: str,
) -> list[dict]:
    sales = []
    start = date.today() - timedelta(days=30)

    for d in range(30):
        day_date = start + timedelta(days=d)

        if pattern == "stable":
            demand = product.base_reorder_quantity + np.random.normal(0, product.base_reorder_quantity * 0.1)

        elif pattern == "trending_up":
            trend_factor = 1 + (0.01 * d)   # grows 1% per day
            demand = product.base_reorder_quantity * trend_factor + np.random.normal(0, product.base_reorder_quantity * 0.1)

        elif pattern == "seasonal":
            # peaks mid-cycle (simulates summer/holiday season)
            seasonal = np.sin(np.pi * d / 30) * product.base_reorder_quantity * 0.5
            demand = product.base_reorder_quantity + seasonal + np.random.normal(0, product.base_reorder_quantity * 0.1)

        elif pattern == "erratic":
            # random spikes — hard to predict, forces model to learn safety stock matters
            demand = product.base_reorder_quantity * np.random.lognormal(0, 0.5)

        demand = max(0, demand)
        returned = int(demand * random.uniform(0, 0.05))  # ~0–5% return rate

        sales.append(SaleRecord(
            id = str(uuid.uuid4()),
            sku = product.sku,
            date = str(day_date),

            units_sold= int(round(demand)),
            units_returned= returned,

            organization_id= product.organization_id,
            product_id= product.id
        ))

    return sales

def generate_order_quantity(sales: list[SaleRecord], product: Product):
    total_sold = sum(s.units_sold - s.units_returned for s in sales)
    avg_weekly_demand = total_sold / len(sales) * 7

    # cover demand during delivery + safety buffer
    demand_during_delivery = avg_weekly_demand * product.delivery_delay / 7
    safety_stock = product.reorder_point * 0.5
    base = demand_during_delivery + safety_stock

    # blend with base reorder quantity
    optimal = 0.6 * base + 0.4 * product.base_reorder_quantity

    # noise
    noise = random.gauss(mu=0, sigma=optimal * 0.1)  # ~10% std deviation
    result = max(1, round(optimal + noise))

    return result

def write():
    data = []
    for _ in range(300):
        product = generate_product()
        saleRecords = generate_sale_records(product, random.choice(PATTERNS))
        orderQuantity = generate_order_quantity(saleRecords, product)

        data.append(MockPredictionPayload(
            sales=saleRecords,
            product=product,
            orderQuantity=orderQuantity
        ))

    with open(DATA_PATH, "w") as f:
        json.dump([entry.model_dump(mode="json") for entry in data], f, indent=2)


