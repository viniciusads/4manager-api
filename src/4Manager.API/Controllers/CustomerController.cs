using _4Tech._4Manager.Application.Features.Customers.Queries;
using _4Tech._4Manager.Application.Features.Customers.Commands;
using _4Tech._4Manager.Application.Features.Customers.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _4Tech._4Manager.API.Controllers
{
    [ApiController]
    [Route("api/v1/client")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> Get()
        {
            var customers = await _mediator.Send(new GetCustomersQuery());
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> GetById(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery(id));
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost("create")]
        public async Task<ActionResult<CustomerResponseDto>> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            var createdCustomer = await _mediator.Send(command);
            if (createdCustomer == null)
            {
                return BadRequest(new { message = "Falha ao cadastrar cliente" });
            }
            return CreatedAtAction(nameof(GetById), new { id = createdCustomer.CustomerId }, createdCustomer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> Update(Guid id, [FromBody] UpdateCustomerCommand command)
        {
            command.CustomerId = id;
            var updatedCustomer = await _mediator.Send(command);
            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteCustomerCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}