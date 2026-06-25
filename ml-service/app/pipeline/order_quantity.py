from app.schemas.product import Product
import numpy as np

def calculate_order_quantity(
        predicted_weekly_demand: float,
        product: Product,
        demand_std: float,
        service_level_z: float = 1.65 # 95% service level formula
) -> int:
    lead_time_weeks = product.delivery_delay / 7

    safety_stock = service_level_z * demand_std * np.sqrt(lead_time_weeks)

    demand_during_lead = predicted_weekly_demand * lead_time_weeks
    reorder_up_to = demand_during_lead + safety_stock

    units_needed = max(0, reorder_up_to - product.current_stock)

    if (units_needed > 0):
        units_needed = max(units_needed, product.base_reorder_quantity)

    return int(np.ceil(units_needed))