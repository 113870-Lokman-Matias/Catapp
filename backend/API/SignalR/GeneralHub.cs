using Microsoft.AspNetCore.SignalR;

public class GeneralHub : Hub
{
    // Mensaje de Categoria
    public async Task EnviarMensajeCrudCategoria(string mensaje)
    {
        await Clients.All.SendAsync("MensajeCrudCategoria", mensaje);
    }

    // Mensaje de Cotizacion
    public async Task EnviarMensajeUpdateCotizacion(string mensaje)
    {
        await Clients.All.SendAsync("MensajeUpdateCotizacion", mensaje);
    }

    // Mensaje de Costo Envio
    public async Task EnviarMensajeUpdateCostoEnvio(string mensaje)
    {
        await Clients.All.SendAsync("MensajeUpdateCostoEnvio", mensaje);
    }

    // Mensaje de Vendedor
    public async Task EnviarMensajeCrudVendedor(string mensaje)
    {
        await Clients.All.SendAsync("MensajeCrudVendedor", mensaje);
    }

    // Mensaje de Detalle de stock
    public async Task EnviarMensajeCreateDetalleStock(string mensaje)
    {
        await Clients.All.SendAsync("MensajeCreateDetalleStock", mensaje);
    }

    // Mensaje de Pedido
    public async Task EnviarMensajeCrudPedido(string mensaje)
    {
        await Clients.All.SendAsync("MensajeCrudPedido", mensaje);
    }
}