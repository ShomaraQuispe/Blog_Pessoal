global using Xunit;
//desabiliar testes paralelos
[assembly: CollectionBehavior(DisableTestParallelization = true)]
//habilita ordena��o de teste
[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]