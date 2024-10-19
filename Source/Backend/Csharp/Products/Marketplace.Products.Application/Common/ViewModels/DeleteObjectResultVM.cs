using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Products.Application.Common.ViewModels;

/// <summary>
/// Модель результата удаления объекта
/// </summary>
/// <param name="Deleted">Удален ли объект</param>
public record DeleteObjectResultVM(bool Deleted);
