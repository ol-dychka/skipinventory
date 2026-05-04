export interface SaleBatchCreateRequest {
  data: SaleRecordInput[];
  date: string;
}

export interface SaleRecordInput {
  id: string;
  name: string;
  sku: string;
  quantity: number;
}
