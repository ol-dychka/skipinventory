export interface CreateProductRequest {
  name: string;
  sku: string | undefined;
  vendor: string;
  costPrice: number;
  salePrice: number;
  currentStock: number;
  reorderPoint: number | undefined;
  baseReorderQuantity: number;
  deliveryDelay: number | undefined;
  category: string | undefined;
}
