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

export interface SaleRecordSummary {
  days: SaleRecordSummaryLine[];
}

export interface SaleRecordSummaryLine {
  date: Date;
  total: number;
}
