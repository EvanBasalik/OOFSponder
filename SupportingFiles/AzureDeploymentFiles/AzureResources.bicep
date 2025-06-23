resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: 'OOFSponderLogAnalytics'
  location: resourceGroup().location
  sku: {
    name: 'PerGB2018'
  }
  properties: {
    retentionInDays: 30
  }
  tags: {
    environment: 'production'
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: 'OOFSponderInsights'
  location: resourceGroup().location
  kind: 'web'
  properties: {
    Application_Type: type
    Request_Source: requestSource
    WorkspaceResourceId: logAnalytics.id
  }
  tags: {
    environment: 'production'
  }
}