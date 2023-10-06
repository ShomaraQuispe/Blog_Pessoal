global using Xunit;
//desabiliar testes paralelos
[assembly: CollectionBehavior(DisableTestParallelization = true)]
//habilita ordenação de teste
[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]