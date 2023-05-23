targetScope='subscription'

param location string
@secure()
param publisherEmail string
@secure()
param publisherName string

var rgName = 'rg-apim-soap-passthru'

var suffix = uniqueString(rg.id)

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: rgName
  location: location
}

module monitoring 'modules/monitoring/monitoring.bicep' = {
  scope: resourceGroup(rg.name)
  name: 'monitoring'
  params: {
    location: location
    suffix: suffix
  }
}

module storageFunction 'modules/storage/storage.bicep' = {
  scope: resourceGroup(rg.name)
  name: 'storage'
  params: {
    location: location
    storageName: 'strf${suffix}'
  }
}

module function 'modules/function/function.bicep' = {
  scope: resourceGroup(rg.name)
  name: 'function'
  params: {
    location: location
    suffix: suffix
    appInsightName: monitoring.outputs.appInsightName
    storageName: storageFunction.outputs.name
  }
}

module asp 'modules/webApp/appServicePlan.bicep' = {
  scope: resourceGroup(rg.name)
  name: 'asp'
  params: {
    location: location
    suffix: suffix
  }
}

module soapService 'modules/webApp/soapService.bicep' = {
  scope: resourceGroup(rg.name)
  name: 'soapService'
  params: {
    appInsightName: monitoring.outputs.appInsightName
    appServiceId: asp.outputs.aspId
    location: location
    suffix: suffix
  }
}

module apim 'modules/apim/apim.bicep' = {
  scope: resourceGroup(rg.name)
  name: 'apim'
  params: {
    appInsightName: monitoring.outputs.appInsightName
    location: location
    publisherEmail: publisherEmail
    publisherName: publisherName
    suffix: suffix
  }
}

output functionName string = function.outputs.functionOutputName
output apimName string = apim.outputs.apimName
output rgName string = rg.name
