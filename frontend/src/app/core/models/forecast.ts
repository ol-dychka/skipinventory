export interface Forecast {
  id: string;
  sku: string;
  generatedAt: Date;
  forecastStart: Date;
  forecastEnd: Date;
  predictedQuantity: number;
  suggestReorder: number;
  productId: string;
}
