# msgraph-files-sample
Projeto de exemplo para acesso ao OneDrive for Business usando Microsoft Graph



Para o projeto funcionar corretamente, será necessário alterar as propriedades ClienteId e ClientSecret no appsettings.json do projeto.

```sh
 "ClientId": "{CLIENTE_ID}}",
 "ClientSecret": "{CLIENTE_SECRET}",
```


Para obter essas informações é preciso ter uma aplicação cadastrada no Microsft Graph:
- https://docs.microsoft.com/pt-br/graph/overview
- https://docs.microsoft.com/pt-br/graph/auth-overview


LIMITAÇÕES (OneDrive for Business e SharePoint):
-------------

#### Pesquisa de arquivos
	A pesquisa não retornará as seguintes propriedades:
        > createdBy
        > modifiedBy
        > parentReference


#### Filtrando
	A filtragem de uma coleção de itens só pode ser feita pelo nome e pelas propriedades do URL .

#### Classificação
	A string de consulta orderby só funciona com as propriedades name e url .

#### Compartilhado por mim
	Compartilhado por Mim não está disponível.

#### Upload de itens
	O upload de URL está disponível apenas para URIs de dados. Não suporta o envio de URLs HTTP ou HTTPS.
    O upload multipartes não está disponível.

#### Visualizar deltas
	A ação delta só está disponível no rootitem de uma unidade, por exemplo /drive/root.
	As seguintes propriedades não são retornadas:
		> createdBy
		> cTag
		> lastModifiedBy