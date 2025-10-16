namespace CatalogAPI.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

// TODO: Add fluent validators for all commands

internal class DeleteProductCommandHandler(IDocumentSession _session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        _session.Delete<Product>(command.Id);

        await _session.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}
