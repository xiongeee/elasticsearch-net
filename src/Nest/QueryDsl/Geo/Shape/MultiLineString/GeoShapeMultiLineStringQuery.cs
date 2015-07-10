﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Nest.Resolvers;
using Newtonsoft.Json;
using Elasticsearch.Net;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface IGeoShapeMultiLineStringQuery : IGeoShapeQuery
	{
		[JsonProperty("shape")]
		IMultiLineStringGeoShape Shape { get; set; }
	}

	public class GeoShapeMultiLineStringQuery : FieldNameQuery, IGeoShapeMultiLineStringQuery
	{
		bool IQuery.Conditionless => IsConditionless(this);
		public IMultiLineStringGeoShape Shape { get; set; }

		protected override void WrapInContainer(IQueryContainer c) => c.GeoShape = this;
		internal static bool IsConditionless(IGeoShapeMultiLineStringQuery q) => q.Field.IsConditionless() || q.Shape == null || !q.Shape.Coordinates.HasAny();
	}

	public class GeoShapeMultiLineStringQueryDescriptor<T> 
		: FieldNameQueryDescriptor<GeoShapeMultiLineStringQueryDescriptor<T>, IGeoShapeMultiLineStringQuery, T>
		, IGeoShapeMultiLineStringQuery where T : class
	{
		private IGeoShapeMultiLineStringQuery Self => this;
		bool IQuery.Conditionless => GeoShapeMultiLineStringQuery.IsConditionless(this);
		IMultiLineStringGeoShape IGeoShapeMultiLineStringQuery.Shape { get; set; }

		public GeoShapeMultiLineStringQueryDescriptor<T> Coordinates(IEnumerable<IEnumerable<IEnumerable<double>>> coordinates)
		{
			if (Self.Shape == null)
				Self.Shape = new MultiLineStringGeoShape();
			Self.Shape.Coordinates = coordinates;
			return this;
		}
	}
}