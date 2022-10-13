using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAN_Simulator
{
  class CanUnknownException : Exception
  {
    public CanUnknownException()
      : base("Неизвестная ошибка.")
    {
    }
  }

  class CanInitializationException : Exception
  {
    public CanInitializationException()
      : base("Библиотека CHAI не инициализарована.")
    {
    }
  }

  class CanGenericException : Exception
  {
    public CanGenericException()
      : base("Обычная (не специфицированная) ошибка.")
    {
    }
  }

  class CanDeviceIsBusyException : Exception
  {
    public CanDeviceIsBusyException()
      : base("Устройство занято.")
    {
    }
  }

  class CanMemoryFaultException : Exception
  {
    public CanMemoryFaultException()
      : base("Ошибка памяти.")
    {
    }
  }

  class CanIncorrectStateException : Exception
  {
    public CanIncorrectStateException()
      : base("Функция не может быть вызвана в данном состоянии устройства.")
    {
    }
  }

  class CanInvalidCallException : Exception
  {
    public CanInvalidCallException()
      : base("Неверный вызов. Функция не может быть вызвана для этого объекта.")
    {
    }
  }

  class CanInvalidParameterException : Exception
  {
    public CanInvalidParameterException()
      : base("Неверные параметры.")
    {
    }
  }

  class CanCannotAccessException : Exception
  {
    public CanCannotAccessException()
      : base("Ресурс недоступен.")
    {
    }
  }

  class CanNotImplementedException : Exception
  {
    public CanNotImplementedException()
      : base("Функция или свойство не реализована.")
    {
    }
  }

  class CanIOErrorException : Exception
  {
    public CanIOErrorException()
      : base("Ошибка ввода/вывода.")
    {
    }
  }

  class CanNoDeviceException : Exception
  {
    public CanNoDeviceException()
      : base("Устройство не найдено.")
    {
    }
  }

  class CanCallWasInterruptedException : Exception
  {
    public CanCallWasInterruptedException()
      : base("Вызов был прерван событием.")
    {
    }
  }

  class CanNoResourcesException : Exception
  {
    public CanNoResourcesException()
      : base("Ресурс не найден.")
    {
    }
  }

  class CanTimeoutException : Exception
  {
    public CanTimeoutException()
      : base("Таймаут.")
    {
    }
  }
}
