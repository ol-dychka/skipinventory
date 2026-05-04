export interface SaleBatchCreateRequest {
  data: SaleRecordInput[];
  date?: Date;
}

export interface SaleRecordInput {
  id: string;
  name: string;
  sku: string;
  quantity: number;
}
