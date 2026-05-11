# Feature                   Description

# avg_net_sales_4w          Avg (sold - returned) over last 4 weeks
# avg_net_sales_8w          Longer window captures trend
# trend                     4w avg - 8w avg (positive = growing demand)
# sales_std_4w              Demand volatility (used in safety stock)
# weeks_since_last_sale     Staleness signal                                    (N/A)
# stock_coverage_days       current_stock / avg_daily_demand                    (N/A)
# delivery_delay            From Product, affects how far ahead to order
# reorder_point             (normalized) Domain knowledge as a feature
# current_stock             From Product, not to go over stock
# category_encoded          One-hot or ordinal                                  (N/A)

import numpy as np
import pandas as pd
from schemas.sale_record import SaleRecord
from schemas.product import Product

def build_features(sales: list[SaleRecord], product: Product) -> np.ndarray:
    df = pd.DataFrame([s.model_dump() for s in sales])
    df['net_sales'] = df['units_sold'] - df['units_returned']
    df['week'] = pd.to_datetime(df['date']).dt.isocalendar().week

    weekly = df.groupby('week')['net_sales'].sum().sort_index()

    avg_4w = weekly.tail(4).mean()
    avg_8w = weekly.tail(8).mean() if len(weekly) >= 8 else avg_4w
    std_4w = weekly.tail(4).std() or 0.0

    return np.array([
        avg_4w,
        avg_8w,
        avg_4w - avg_8w,
        std_4w,
        product.delivery_delay,
        product.reorder_point,
        product.current_stock
    ])