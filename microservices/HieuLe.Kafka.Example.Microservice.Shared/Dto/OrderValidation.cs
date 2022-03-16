// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.0.0
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace HieuLe.Kafka.Example.Microservice.Shared.Dtos
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Avro;
	using Avro.Specific;
	
	public partial class OrderValidation : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse(@"{""type"":""record"",""name"":""OrderValidation"",""namespace"":""HieuLe.Kafka.Example.Microservice.Shared.Dtos"",""fields"":[{""name"":""orderId"",""type"":""string""},{""name"":""checkType"",""type"":{""type"":""enum"",""name"":""OrderValidationType"",""namespace"":""HieuLe.Kafka.Example.Microservice.Shared.Dtos"",""symbols"":[""INVENTORY_CHECK"",""FRAUD_CHECK"",""ORDER_DETAILS_CHECK""]}},{""name"":""validationResult"",""type"":{""type"":""enum"",""name"":""OrderValidationResult"",""namespace"":""HieuLe.Kafka.Example.Microservice.Shared.Dtos"",""symbols"":[""PASS"",""FAIL"",""ERROR""]}}]}");
		private string _orderId;
		private HieuLe.Kafka.Example.Microservice.Shared.Dtos.OrderValidationType _checkType;
		private HieuLe.Kafka.Example.Microservice.Shared.Dtos.OrderValidationResult _validationResult;
		public virtual Schema Schema
		{
			get
			{
				return OrderValidation._SCHEMA;
			}
		}
		public string orderId
		{
			get
			{
				return this._orderId;
			}
			set
			{
				this._orderId = value;
			}
		}
		public HieuLe.Kafka.Example.Microservice.Shared.Dtos.OrderValidationType checkType
		{
			get
			{
				return this._checkType;
			}
			set
			{
				this._checkType = value;
			}
		}
		public HieuLe.Kafka.Example.Microservice.Shared.Dtos.OrderValidationResult validationResult
		{
			get
			{
				return this._validationResult;
			}
			set
			{
				this._validationResult = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.orderId;
			case 1: return this.checkType;
			case 2: return this.validationResult;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.orderId = (System.String)fieldValue; break;
			case 1: this.checkType = (HieuLe.Kafka.Example.Microservice.Shared.Dtos.OrderValidationType)fieldValue; break;
			case 2: this.validationResult = (HieuLe.Kafka.Example.Microservice.Shared.Dtos.OrderValidationResult)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
