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

export interface EditProductRequest {
  name: string;
  sku: string | undefined;
  vendor: string;
  costPrice: number;
  salePrice: number;
  currentStock: number;
  reorderPoint: number;
  baseReorderQuantity: number;
  deliveryDelay: number;
  category: string | undefined;
  isActive: boolean;
  // unit
  // currency
}

export interface ProductModel {
  id: string;
  name: string;
  sku: string;
  category: string | undefined;
  vendor: string;
  unit: string;

  costPrice: number;
  salePrice: number;
  currency: string;

  currentStock: number;
  reorderPoint: number;
  baseReorderQuantity: number;
  deliveryDelay: number;

  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}
