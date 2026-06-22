export function displayDate(date: Date) {
  date = new Date(date);
  return date.toISOString().split('T')[0];
}
