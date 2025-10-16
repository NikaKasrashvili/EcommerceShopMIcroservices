namespace CatalogAPI.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

// TODO: Add fluent validators for all commands

internal class DeleteProductCommandHandler(IDocumentSession _session, ILogger<DeleteProductCommandHandler> _logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteProductCommand {@Command}", command);
        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product is null)
        {
            _logger.LogInformation("Product with this id not found");
            throw new ProductNotFoundException(command.Id);
        }

        _session.Delete<Product>(command.Id);

        await _session.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}
