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

    // Mensaje de Envio
    public async Task EnviarMensajeUpdateEnvio(string mensaje)
    {
        await Clients.All.SendAsync("MensajeCrudEntrega", mensaje);
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

    // Mensajes de Pedidos
    public async Task EnviarMensajeCreatePedido(string mensaje)
    {
        await Clients.All.SendAsync("MensajeCreatePedido", mensaje);
    }

    public async Task EnviarMensajeUpdateDeletePedido(string mensaje)
    {
        await Clients.All.SendAsync("MensajeUpdateDeletePedido", mensaje);
    }

    // Mensaje de Producto
    public async Task EnviarMensajeCrudProducto(string mensaje)
    {
        await Clients.All.SendAsync("MensajeCrudProducto", mensaje);
    }

    // Mensaje de Metodo de pago
    public async Task EnviarMensajeCrudMetodoPago(string mensaje)
    {
        await Clients.All.SendAsync("MensajeCrudMetodoPago", mensaje);
    }

    // Mensaje de Configuracion
    public async Task EnviarMensajeUpdateConfiguracion(string mensaje)
    {
        await Clients.All.SendAsync("MensajeUpdateConfiguracion", mensaje);
    }

    // Mensaje de Subcategoria
    public async Task EnviarMensajeCrudSubcategoria(string mensaje)
    {
        await Clients.All.SendAsync("MensajeCrudSubcategoria", mensaje);
    }
}