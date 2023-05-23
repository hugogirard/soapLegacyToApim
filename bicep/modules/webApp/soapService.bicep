param suffix string
param location string
param appInsightName string
param appServiceId string 

resource insight 'Microsoft.Insights/components@2020-02-02' existing = {
  name: appInsightName
}

resource soap 'Microsoft.Web/sites@2022-09-01' = {
  name: 'soap-${suffix}'
  location: location
  properties: {                
    siteConfig: {           
      appSettings: [
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: insight.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: insight.properties.ConnectionString
        }     
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~2'
        }             
        {
          name: 'XDT_MicrosoftApplicationInsights_Mode'
          value: 'default'
        }               
      ] 
      netFrameworkVersion: 'v6.0'                 
    }          
    serverFarmId: appServiceId 
    httpsOnly: true
  } 
}

output soapWebName string = soap.name
