param location string
param suffix string

resource asp 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: 'asp-${suffix}'
  location: location
  sku: {
    name: 'B1'
  }
}

output aspId string = asp.id
