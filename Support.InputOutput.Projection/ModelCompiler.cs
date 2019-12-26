//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Reflection.Emit;
//using Noname.BitConversion;
//using Noname.BitConversion.System;
//using Noname.IO.ObjectOrientedDomain;

//namespace Noname.IO.ObjectOrientedDatabase
//{
//	internal sealed class ModelCompiler
//	{
//		private sealed class ReferenceTypeCompiler
//		{
//			private abstract class VersionCompiler
//			{
//				internal sealed class DataConstantLengthCompiler
//				{
//					static private readonly Type _bitConverterConstantLengthTypeDefinition;
//					static private readonly Type _bitConverterConstantLengthBuilderTypeDefinition;

//					static DataConstantLengthCompiler()
//					{
//						_bitConverterConstantLengthTypeDefinition = typeof(ConstantLengthBitConverter<>);
//						_bitConverterConstantLengthBuilderTypeDefinition = typeof(ConstantLengthBitConverterBuilder<>);
//					}

//					internal readonly FieldBuilder[] _fieldBuilders;
//					internal readonly Type _type;
//					internal readonly FieldBuilder _bitConverterBuilderTypeInstanceFieldBuilder;

//					internal DataConstantLengthCompiler(VersionCompiler versionCompiler, List<PropertyDataConstantLengthBuilder> propertyBuilders)
//					{
//						TypeBuilder typeBuilder = versionCompiler._typeBuilder.DefineNestedType("DataConstantLength", TypeAttributes.NestedPublic | TypeAttributes.Sealed | TypeAttributes.SequentialLayout, _valueTypeType);
//						int fieldBuilderCount = propertyBuilders.Count;
//						_fieldBuilders = new FieldBuilder[fieldBuilderCount];
//						for (int fieldBuilderIndex = 0; fieldBuilderIndex < fieldBuilderCount; fieldBuilderIndex++)
//						{
//							PropertyBuilder propertyBuilder;
//							_fieldBuilders[fieldBuilderIndex] = typeBuilder.DefineField((propertyBuilder = propertyBuilders[fieldBuilderIndex])._propertyInfo.Name, propertyBuilder._propertyInfo.PropertyType, FieldAttributes.Public);
//						}
//						_type = typeBuilder.CreateType();
//						Type bitConverterConstantLengthBuilderType = _bitConverterConstantLengthBuilderTypeDefinition.MakeGenericType(_type);
//						TypeBuilder bitConverterBuilderTypeBuilder = typeBuilder.DefineNestedType("BitConverterBuilder", TypeAttributes.NestedPublic | TypeAttributes.Abstract | TypeAttributes.Sealed);
//						_bitConverterBuilderTypeInstanceFieldBuilder = bitConverterBuilderTypeBuilder.DefineField("_instance", _bitConverterConstantLengthTypeDefinition.MakeGenericType(_type), FieldAttributes.Static | FieldAttributes.Public);
//						object bitConverterBuilder = Activator.CreateInstance(bitConverterConstantLengthBuilderType);
//						MethodInfo bitConverterConstantLengthBuilderAddFieldMethodInfo = bitConverterConstantLengthBuilderType.GetMethod(nameof(ConstantLengthBitConverterBuilder<byte>.AddField), BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { _fieldInfoType, _objectType }, null);
//						object[] arguments = new object[2];
//						for (int fieldIndex = 0; fieldIndex < fieldBuilderCount; fieldIndex++)
//						{
//							arguments[0] = _type.GetField(_fieldBuilders[fieldIndex].Name);
//							arguments[1] = propertyBuilders[fieldIndex]._bitConverter;
//							bitConverterConstantLengthBuilderAddFieldMethodInfo.Invoke(bitConverterBuilder, arguments);
//						}
//						bitConverterBuilderTypeBuilder.CreateType().GetField(_bitConverterBuilderTypeInstanceFieldBuilder.Name).SetValue(null, bitConverterConstantLengthBuilderType.GetProperty(nameof(ConstantLengthBitConverterBuilder<byte>.Instance)).GetValue(bitConverterBuilder));
//					}
//				}
//				internal sealed class DataVariableLengthCompiler
//				{
//					static private readonly Type _bitConverterVariableLengthTypeDefinition;
//					static private readonly Type _bitConverterVariableLengthBuilderTypeDefinition;

//					static DataVariableLengthCompiler()
//					{
//						_bitConverterVariableLengthTypeDefinition = typeof(VariableLengthBitConverter<>);
//						_bitConverterVariableLengthBuilderTypeDefinition = typeof(VariableLengthBitConverterBuilder<>);
//					}

//					internal readonly FieldBuilder[] _fieldBuilders;
//					internal readonly Type _type;
//					internal readonly FieldBuilder _bitConverterBuilderTypeInstanceFieldBuilder;

//					internal DataVariableLengthCompiler(VersionCompiler versionCompiler, List<PropertyDataVariableLengthBuilder> propertyBuilders)
//					{
//						TypeBuilder typeBuilder = versionCompiler._typeBuilder.DefineNestedType("DataVariableLength", TypeAttributes.NestedPublic | TypeAttributes.Sealed | TypeAttributes.SequentialLayout, _valueTypeType);
//						int fieldBuilderCount = propertyBuilders.Count;
//						_fieldBuilders = new FieldBuilder[fieldBuilderCount];
//						for (int fieldBuilderIndex = 0; fieldBuilderIndex < fieldBuilderCount; fieldBuilderIndex++)
//						{
//							PropertyBuilder propertyBuilder;
//							_fieldBuilders[fieldBuilderIndex] = typeBuilder.DefineField((propertyBuilder = propertyBuilders[fieldBuilderIndex])._propertyInfo.Name, propertyBuilder._propertyInfo.PropertyType, FieldAttributes.Public);
//						}
//						_type = typeBuilder.CreateType();
//						Type bitConverterVariableLengthBuilderType = _bitConverterVariableLengthBuilderTypeDefinition.MakeGenericType(_type);
//						TypeBuilder bitConverterBuilderTypeBuilder = typeBuilder.DefineNestedType("BitConverterBuilder", TypeAttributes.NestedPublic | TypeAttributes.Abstract | TypeAttributes.Sealed);
//						_bitConverterBuilderTypeInstanceFieldBuilder = bitConverterBuilderTypeBuilder.DefineField("_instance", _bitConverterVariableLengthTypeDefinition.MakeGenericType(_type), FieldAttributes.Static | FieldAttributes.Public);
//						object bitConverterBuilder = Activator.CreateInstance(bitConverterVariableLengthBuilderType);
//						MethodInfo bitConverterVariableLengthBuilderAddFieldMethodInfo = bitConverterVariableLengthBuilderType.GetMethod(nameof(VariableLengthBitConverterBuilder<byte>.AddField), BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { _fieldInfoType, _objectType }, null);
//						object[] arguments = new object[2];
//						for (int fieldIndex = 0; fieldIndex < fieldBuilderCount; fieldIndex++)
//						{
//							arguments[0] = _type.GetField(_fieldBuilders[fieldIndex].Name);
//							arguments[1] = propertyBuilders[fieldIndex]._bitConverter;
//							bitConverterVariableLengthBuilderAddFieldMethodInfo.Invoke(bitConverterBuilder, arguments);
//						}
//						bitConverterBuilderTypeBuilder.CreateType().GetField(_bitConverterBuilderTypeInstanceFieldBuilder.Name).SetValue(null, bitConverterVariableLengthBuilderType.GetProperty(nameof(VariableLengthBitConverterBuilder<byte>.Instance)).GetValue(bitConverterBuilder));
//					}
//				}
//				internal sealed class DescriptorReferenceCompiler
//				{
//					internal readonly int _pointerIndex;
//					internal readonly FieldBuilder _fieldBuilder;
//					internal readonly FieldBuilder _loadedFieldBuilder;

//					internal DescriptorReferenceCompiler(int pointerIndex, FieldBuilder fieldBuilder, FieldBuilder loadedFieldBuilder)
//					{
//						_pointerIndex = pointerIndex;
//						_fieldBuilder = fieldBuilder;
//						_loadedFieldBuilder = loadedFieldBuilder;
//					}
//				}
//				internal sealed class DescriptorPropertyCompiler
//				{
//					internal readonly MethodBuilder _getMethodBuilder;
//					internal readonly MethodBuilder _setMethodBuilder;

//					internal DescriptorPropertyCompiler(MethodBuilder getMethodBuilder, MethodBuilder setMethodBuilder)
//					{
//						_getMethodBuilder = getMethodBuilder;
//						_setMethodBuilder = setMethodBuilder;
//					}
//				}

//				internal readonly ReferenceTypeCompiler _referenceTypeCompiler;
//				internal readonly MethodInfo _translateMethodInfo;
//				internal readonly int _referenceCount;
//				internal readonly TypeBuilder _typeBuilder;
//				internal readonly DataConstantLengthCompiler _dataConstantLengthCompiler;
//				internal readonly DataVariableLengthCompiler _dataVariableLengthCompiler;
//				internal readonly Type _objectManagerType;
//				internal readonly MethodInfo _objectManagerTypePointerPropertyGetMethodInfo;
//				internal readonly MethodInfo _objectManagerTypeDataConstantLengthPropertyGetMethodInfo;
//				internal readonly MethodInfo _objectManagerTypeDataConstantLengthPropertySetMethodInfo;
//				internal readonly MethodInfo _objectManagerTypeDataVariableLengthPropertyGetMethodInfo;
//				internal readonly MethodInfo _objectManagerTypeDataVariableLengthPropertySetMethodInfo;
//				internal readonly MethodInfo _objectManagerTypeItemPropertyGetMethodInfo;
//				internal readonly MethodInfo _objectManagerTypeItemPropertySetMethodInfo;
//				internal readonly MethodInfo _objectManagerActivatorTypeCreateMethodInfo;
//				internal readonly MethodInfo _objectManagerActivatorTypeOpenMethodInfo;
//				internal readonly TypeBuilder _descriptorTypeBuilder;
//				internal readonly FieldBuilder _descriptorTypeValueManagerFieldBuilder;
//				internal readonly FieldBuilder _descriptorTypeDataConstantLengthFieldBuilder;
//				internal readonly FieldBuilder _descriptorTypeDataVariableLengthFieldBuilder;
//				internal readonly FieldBuilder _descriptorTypeDataVariableLengthLoadedFieldBuilder;
//				internal readonly DescriptorReferenceCompiler[] _descriptorTypeReferenceSingleCompilers;
//				internal readonly MethodBuilder _descriptorTypeLoadDataVariableLengthMethodBuilder;
//				internal readonly DescriptorPropertyCompiler[] _descriptorTypeDataConstantLengthPropertyCompilers;
//				internal readonly DescriptorPropertyCompiler[] _descriptorTypeDataVariableLengthPropertyCompilers;
//				internal readonly DescriptorPropertyCompiler[] _descriptorTypeReferenceSinglePropertyCompilers;
//				internal readonly TypeBuilder _implementationTypeBuilder;
//				internal readonly FieldInfo _implementationTypeDescriptorFieldBuilder;
//				internal readonly ConstructorBuilder _implementationTypeConstructorBuilder;

//				internal VersionCompiler(ReferenceTypeCompiler referenceTypeCompiler, TypeVersionBuilderBase versionBuilder, int index)
//				{
//					_referenceTypeCompiler = referenceTypeCompiler;
//					_translateMethodInfo = versionBuilder._translateMethodInfo;
//					_referenceCount = versionBuilder.ReferenceCount;
//					List<PropertyDataConstantLengthBuilder> dataConstantLengthBuilders = new List<PropertyDataConstantLengthBuilder>(versionBuilder.DataConstantLengthBuilders);
//					List<PropertyDataVariableLengthBuilder> dataVariableLengthBuilders = new List<PropertyDataVariableLengthBuilder>(versionBuilder.DataVariableLengthBuilders);
//					List<PropertyDatabaseTypeSingleBuilder> referenceSingleBuilders = new List<PropertyDatabaseTypeSingleBuilder>(versionBuilder.ReferenceSingleBuilders);
//					(_typeBuilder = referenceTypeCompiler._typeBuilder.DefineNestedType(index.ToString(), TypeAttributes.NestedPublic | TypeAttributes.Abstract | TypeAttributes.Sealed)).CreateType();
//					_dataConstantLengthCompiler = new DataConstantLengthCompiler(this, dataConstantLengthBuilders);
//					_dataVariableLengthCompiler = new DataVariableLengthCompiler(this, dataVariableLengthBuilders);
//					_objectManagerType = _objectManagerTypeDefinition.MakeGenericType(_dataConstantLengthCompiler._type, _dataVariableLengthCompiler._type);
//					_objectManagerTypePointerPropertyGetMethodInfo = _objectManagerType.GetProperty(nameof(ValueManager<byte, byte>.Pointer)).GetGetMethod();
//					PropertyInfo objectManagerTypeDataConstantLengthPropertyInfo = _objectManagerType.GetProperty(nameof(ValueManager<byte, byte>.DataConstantLength));
//					_objectManagerTypeDataConstantLengthPropertyGetMethodInfo = objectManagerTypeDataConstantLengthPropertyInfo.GetGetMethod();
//					_objectManagerTypeDataConstantLengthPropertySetMethodInfo = objectManagerTypeDataConstantLengthPropertyInfo.GetSetMethod();
//					PropertyInfo objectManagerTypeDataVariableLengthPropertyInfo = _objectManagerType.GetProperty(nameof(ValueManager<byte, byte>.DataVariableLength));
//					_objectManagerTypeDataVariableLengthPropertyGetMethodInfo = objectManagerTypeDataVariableLengthPropertyInfo.GetGetMethod();
//					_objectManagerTypeDataVariableLengthPropertySetMethodInfo = objectManagerTypeDataVariableLengthPropertyInfo.GetSetMethod();
//					PropertyInfo objectManagerTypeItemPropertyInfo = _objectManagerType.GetProperty("Item");
//					_objectManagerTypeItemPropertyGetMethodInfo = objectManagerTypeItemPropertyInfo.GetGetMethod();
//					_objectManagerTypeItemPropertySetMethodInfo = objectManagerTypeItemPropertyInfo.GetSetMethod();
//					_objectManagerActivatorTypeCreateMethodInfo = _objectManagerActivatorType.GetMethod(nameof(ValueManagerActivator.Create)).MakeGenericMethod(_dataConstantLengthCompiler._type, _dataVariableLengthCompiler._type);
//					_objectManagerActivatorTypeOpenMethodInfo = _objectManagerActivatorType.GetMethod(nameof(ValueManagerActivator.Open)).MakeGenericMethod(_dataConstantLengthCompiler._type, _dataVariableLengthCompiler._type);
//					_descriptorTypeBuilder = _typeBuilder.DefineNestedType("Descriptor", TypeAttributes.NestedPublic | TypeAttributes.Sealed);
//					_descriptorTypeValueManagerFieldBuilder = _descriptorTypeBuilder.DefineField("_objectManager", _objectManagerType, FieldAttributes.PrivateScope);
//					_descriptorTypeDataConstantLengthFieldBuilder = _descriptorTypeBuilder.DefineField("_dataConstantLength", _dataConstantLengthCompiler._type, FieldAttributes.Private);
//					_descriptorTypeDataVariableLengthFieldBuilder = _descriptorTypeBuilder.DefineField("_dataVariableLength", _dataVariableLengthCompiler._type, FieldAttributes.Private);
//					_descriptorTypeDataVariableLengthLoadedFieldBuilder = _descriptorTypeBuilder.DefineField("_dataVariableLengthLoaded", _booleanType, FieldAttributes.Private);
//					int pointerIndex = 0;
//					int referenceSinglePropertyCount = referenceSingleBuilders.Count;
//					_descriptorTypeReferenceSingleCompilers = new DescriptorReferenceCompiler[referenceSinglePropertyCount];
//					for (int referenceSinglePropertyIndex = 0; referenceSinglePropertyIndex < referenceSinglePropertyCount; referenceSinglePropertyIndex++, pointerIndex++)
//					{
//						PropertyInfo propertyInfo = referenceSingleBuilders[referenceSinglePropertyIndex]._propertyInfo;
//						_descriptorTypeReferenceSingleCompilers[referenceSinglePropertyIndex] = new DescriptorReferenceCompiler(pointerIndex, _descriptorTypeBuilder.DefineField(propertyInfo.Name, propertyInfo.PropertyType, FieldAttributes.Private), _descriptorTypeBuilder.DefineField(propertyInfo.Name + "Loaded", _booleanType, FieldAttributes.Private));
//					}
//					_descriptorTypeLoadDataVariableLengthMethodBuilder = _descriptorTypeBuilder.DefineMethod("LoadDataVariableLength", MethodAttributes.Private | MethodAttributes.HideBySig, _voidType, Type.EmptyTypes);
//					ILGenerator ilGenerator = _descriptorTypeLoadDataVariableLengthMethodBuilder.GetILGenerator();
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Dup);
//					ilGenerator.Emit(OpCodes.Dup);
//					ilGenerator.Emit(OpCodes.Ldfld, _descriptorTypeValueManagerFieldBuilder);
//					ilGenerator.Emit(OpCodes.Call, _objectManagerTypeDataVariableLengthPropertyGetMethodInfo);
//					ilGenerator.Emit(OpCodes.Stfld, _descriptorTypeDataVariableLengthFieldBuilder);
//					ilGenerator.Emit(OpCodes.Ldc_I4, 1);
//					ilGenerator.Emit(OpCodes.Stfld, _descriptorTypeDataVariableLengthLoadedFieldBuilder);
//					ilGenerator.Emit(OpCodes.Ret);
//					int constantLengthPropertyCount = _dataConstantLengthCompiler._fieldBuilders.Length;
//					_descriptorTypeDataConstantLengthPropertyCompilers = new DescriptorPropertyCompiler[constantLengthPropertyCount];
//					for (int constantLengthPropertyIndex = 0; constantLengthPropertyIndex < constantLengthPropertyCount; constantLengthPropertyIndex++)
//					{
//						PropertyInfo propertyInfo = dataConstantLengthBuilders[constantLengthPropertyIndex]._propertyInfo;
//						ilGenerator = (_descriptorTypeDataConstantLengthPropertyCompilers[constantLengthPropertyIndex] = new DescriptorPropertyCompiler(_descriptorTypeBuilder.DefineMethod("get_" + propertyInfo.Name, MethodAttributes.HideBySig, propertyInfo.PropertyType, Type.EmptyTypes), _descriptorTypeBuilder.DefineMethod("set_" + propertyInfo.Name, MethodAttributes.HideBySig, _voidType, new Type[] { propertyInfo.PropertyType })))._getMethodBuilder.GetILGenerator();
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldflda, _descriptorTypeDataConstantLengthFieldBuilder);
//						ilGenerator.Emit(OpCodes.Ldfld, _dataConstantLengthCompiler._fieldBuilders[constantLengthPropertyIndex]);
//						ilGenerator.Emit(OpCodes.Ret);
//					}
//					int variableLengthPropertyCount = _dataVariableLengthCompiler._fieldBuilders.Length;
//					_descriptorTypeDataVariableLengthPropertyCompilers = new DescriptorPropertyCompiler[variableLengthPropertyCount];
//					for (int variableLengthPropertyIndex = 0; variableLengthPropertyIndex < variableLengthPropertyCount; variableLengthPropertyIndex++)
//					{
//						PropertyInfo propertyInfo = dataVariableLengthBuilders[variableLengthPropertyIndex]._propertyInfo;
//						ilGenerator = (_descriptorTypeDataVariableLengthPropertyCompilers[variableLengthPropertyIndex] = new DescriptorPropertyCompiler(_descriptorTypeBuilder.DefineMethod("get_" + propertyInfo.Name, MethodAttributes.HideBySig, propertyInfo.PropertyType, Type.EmptyTypes), _descriptorTypeBuilder.DefineMethod("set_" + propertyInfo.Name, MethodAttributes.HideBySig, _voidType, new Type[] { propertyInfo.PropertyType })))._getMethodBuilder.GetILGenerator();
//						Label returnLabel = ilGenerator.DefineLabel();
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Dup);
//						ilGenerator.Emit(OpCodes.Ldfld, _descriptorTypeDataVariableLengthLoadedFieldBuilder);
//						ilGenerator.Emit(OpCodes.Brtrue, returnLabel);
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Call, _descriptorTypeLoadDataVariableLengthMethodBuilder);
//						ilGenerator.MarkLabel(returnLabel);
//						ilGenerator.Emit(OpCodes.Ldflda, _descriptorTypeDataVariableLengthFieldBuilder);
//						ilGenerator.Emit(OpCodes.Ldfld, _dataVariableLengthCompiler._fieldBuilders[variableLengthPropertyIndex]);
//						ilGenerator.Emit(OpCodes.Ret);
//					}
//					_descriptorTypeReferenceSinglePropertyCompilers = new DescriptorPropertyCompiler[referenceSinglePropertyCount];
//					for (int referenceSinglePropertyIndex = 0; referenceSinglePropertyIndex < referenceSinglePropertyCount; referenceSinglePropertyIndex++)
//					{
//						PropertyInfo propertyInfo = referenceSingleBuilders[referenceSinglePropertyIndex]._propertyInfo;
//						_descriptorTypeReferenceSinglePropertyCompilers[referenceSinglePropertyIndex] = new DescriptorPropertyCompiler(_descriptorTypeBuilder.DefineMethod("get_" + propertyInfo.Name, MethodAttributes.HideBySig, propertyInfo.PropertyType, Type.EmptyTypes), _descriptorTypeBuilder.DefineMethod("set_" + propertyInfo.Name, MethodAttributes.HideBySig, _voidType, new Type[] { propertyInfo.PropertyType }));
//					}
//					_implementationTypeBuilder = _typeBuilder.DefineNestedType("Implementation", TypeAttributes.NestedPublic | TypeAttributes.Sealed, versionBuilder._versionType);
//					_implementationTypeDescriptorFieldBuilder = _implementationTypeBuilder.DefineField("_descriptor", _descriptorTypeBuilder, FieldAttributes.PrivateScope);
//					_implementationTypeConstructorBuilder = _implementationTypeBuilder.DefineConstructor(MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[] { _descriptorTypeBuilder });
//					ilGenerator = _implementationTypeConstructorBuilder.GetILGenerator();
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Call, _objectConstructorInfo);
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Ldarg, 1);
//					ilGenerator.Emit(OpCodes.Stfld, _implementationTypeDescriptorFieldBuilder);
//					ilGenerator.Emit(OpCodes.Ret);
//					for (int constantLengthPropertyIndex = 0; constantLengthPropertyIndex < constantLengthPropertyCount; constantLengthPropertyIndex++)
//						DefineImplementationPropertyOverride(_descriptorTypeDataConstantLengthPropertyCompilers[constantLengthPropertyIndex], dataConstantLengthBuilders[constantLengthPropertyIndex]._propertyInfo);
//					for (int variableLengthPropertyIndex = 0; variableLengthPropertyIndex < variableLengthPropertyCount; variableLengthPropertyIndex++)
//						DefineImplementationPropertyOverride(_descriptorTypeDataVariableLengthPropertyCompilers[variableLengthPropertyIndex], dataVariableLengthBuilders[variableLengthPropertyIndex]._propertyInfo);
//					for (int singleReferencePropertyIndex = 0; singleReferencePropertyIndex < referenceSinglePropertyCount; singleReferencePropertyIndex++)
//						DefineImplementationPropertyOverride(_descriptorTypeReferenceSinglePropertyCompilers[singleReferencePropertyIndex], referenceSingleBuilders[singleReferencePropertyIndex]._propertyInfo);
//				}

//				private void DefineImplementationPropertyOverride(DescriptorPropertyCompiler descriptorPropertyCompiler, PropertyInfo propertyInfo)
//				{
//					MethodBuilder getMethodBuilder = _implementationTypeBuilder.DefineMethod("get_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final, propertyInfo.PropertyType, Type.EmptyTypes);
//					ILGenerator ilGenerator = getMethodBuilder.GetILGenerator();
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Ldfld, _implementationTypeDescriptorFieldBuilder);
//					ilGenerator.Emit(OpCodes.Call, descriptorPropertyCompiler._getMethodBuilder);
//					ilGenerator.Emit(OpCodes.Ret);
//					_implementationTypeBuilder.DefineMethodOverride(getMethodBuilder, propertyInfo.GetGetMethod());
//					MethodBuilder setMethodBuilder = _implementationTypeBuilder.DefineMethod("set_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final, _voidType, new Type[] { propertyInfo.PropertyType });
//					ilGenerator = setMethodBuilder.GetILGenerator();
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Ldfld, _implementationTypeDescriptorFieldBuilder);
//					ilGenerator.Emit(OpCodes.Ldarg, 1);
//					ilGenerator.Emit(OpCodes.Call, descriptorPropertyCompiler._setMethodBuilder);
//					ilGenerator.Emit(OpCodes.Ret);
//					_implementationTypeBuilder.DefineMethodOverride(setMethodBuilder, propertyInfo.GetSetMethod());
//				}
//				internal virtual void PostCompile()
//				{
//					int referenceSinglePropertyCount = _descriptorTypeReferenceSinglePropertyCompilers.Length;
//					for (int singleReferencePropertyIndex = 0; singleReferencePropertyIndex < referenceSinglePropertyCount; singleReferencePropertyIndex++)
//					{
//						DescriptorReferenceCompiler referenceCompiler = _descriptorTypeReferenceSingleCompilers[singleReferencePropertyIndex];
//						ILGenerator ilGenerator = _descriptorTypeReferenceSinglePropertyCompilers[singleReferencePropertyIndex]._getMethodBuilder.GetILGenerator();
//						ilGenerator.DeclareLocal(_pointerManagerType);
//						Label markLabel = ilGenerator.DefineLabel();
//						Label returnLabel = ilGenerator.DefineLabel();
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldfld, referenceCompiler._loadedFieldBuilder);
//						ilGenerator.Emit(OpCodes.Brtrue, returnLabel);
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldfld, _descriptorTypeValueManagerFieldBuilder);
//						ilGenerator.Emit(OpCodes.Ldc_I4, referenceCompiler._pointerIndex);
//						ilGenerator.Emit(OpCodes.Call, _objectManagerTypeItemPropertyGetMethodInfo);
//						ilGenerator.Emit(OpCodes.Dup);
//						ilGenerator.Emit(OpCodes.Stloc, 0);
//						ilGenerator.Emit(OpCodes.Call, _pointerManagerTypeIsNullPropertyGetMethodInfo);
//						ilGenerator.Emit(OpCodes.Brtrue, markLabel);
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldloc, 0);
//						ilGenerator.Emit(OpCodes.Call, _referenceTypeCompiler._modelCompiler._referenceTypeCompilers[referenceCompiler._fieldBuilder.FieldType]._openMethodBuilder);
//						ilGenerator.Emit(OpCodes.Stfld, referenceCompiler._fieldBuilder);
//						ilGenerator.MarkLabel(markLabel);
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldc_I4, 1);
//						ilGenerator.Emit(OpCodes.Stfld, referenceCompiler._loadedFieldBuilder);
//						ilGenerator.MarkLabel(returnLabel);
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldfld, referenceCompiler._fieldBuilder);
//						ilGenerator.Emit(OpCodes.Ret);
//					}
//				}
//			}
//			private sealed class ObsoleteVersionCompiler : VersionCompiler
//			{
//				static private readonly Type _invalidOperationExceptionType;

//				static ObsoleteVersionCompiler() => _invalidOperationExceptionType = typeof(InvalidOperationException);

//				internal readonly ConstructorBuilder _descriptorTypeConstructorBuilder;
//				internal readonly ConstructorBuilder _intermediateImplementationTypeConstructorBuilder;

//				internal ObsoleteVersionCompiler(ReferenceTypeCompiler referenceTypeCompiler, TypeVersionBuilderBase versionBuilder, int index) : base(referenceTypeCompiler, versionBuilder, index)
//				{
//					_descriptorTypeConstructorBuilder = _descriptorTypeBuilder.DefineConstructor(MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[] { _objectManagerType });
//					ILGenerator ilGenerator = _descriptorTypeConstructorBuilder.GetILGenerator();
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Call, _objectConstructorInfo);
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Ldarg, 1);
//					ilGenerator.Emit(OpCodes.Stfld, _descriptorTypeValueManagerFieldBuilder);
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Ldarg, 1);
//					ilGenerator.Emit(OpCodes.Call, _objectManagerTypeDataConstantLengthPropertyGetMethodInfo);
//					ilGenerator.Emit(OpCodes.Stfld, _descriptorTypeDataConstantLengthFieldBuilder);
//					ilGenerator.Emit(OpCodes.Ret);
//					int constantLengthPropertyCount = _descriptorTypeDataConstantLengthPropertyCompilers.Length;
//					for (int constantLengthPropertyIndex = 0; constantLengthPropertyIndex < constantLengthPropertyCount; constantLengthPropertyIndex++)
//						_descriptorTypeDataConstantLengthPropertyCompilers[constantLengthPropertyIndex]._setMethodBuilder.GetILGenerator().ThrowException(_invalidOperationExceptionType);
//					int variableLengthPropertyCount = _descriptorTypeDataVariableLengthPropertyCompilers.Length;
//					for (int variableLengthPropertyIndex = 0; variableLengthPropertyIndex < variableLengthPropertyCount; variableLengthPropertyIndex++)
//						_descriptorTypeDataVariableLengthPropertyCompilers[variableLengthPropertyIndex]._setMethodBuilder.GetILGenerator().ThrowException(_invalidOperationExceptionType);
//					TypeBuilder _intermediateImplementationTypeBuilder = _typeBuilder.DefineNestedType("IntermediateImplementation", TypeAttributes.NestedPublic | TypeAttributes.Sealed, versionBuilder._versionType);
//					(_intermediateImplementationTypeConstructorBuilder = _intermediateImplementationTypeBuilder.DefineConstructor(MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, Type.EmptyTypes)).GetILGenerator().Emit(OpCodes.Ret);
//					int propertyCount = versionBuilder._propertyBuilders.Count;
//					for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
//					{
//						PropertyInfo propertyInfo = versionBuilder._propertyBuilders[propertyIndex]._propertyInfo;
//						FieldBuilder fieldBuilder = _intermediateImplementationTypeBuilder.DefineField(propertyInfo.Name, propertyInfo.PropertyType, FieldAttributes.Private);
//						MethodBuilder getMethodBuilder = _intermediateImplementationTypeBuilder.DefineMethod("get_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final, propertyInfo.PropertyType, Type.EmptyTypes);
//						ilGenerator = getMethodBuilder.GetILGenerator();
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
//						ilGenerator.Emit(OpCodes.Ret);
//						_intermediateImplementationTypeBuilder.DefineMethodOverride(getMethodBuilder, propertyInfo.GetGetMethod());
//						MethodBuilder setMethodBuilder = _intermediateImplementationTypeBuilder.DefineMethod("set_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final, _voidType, new Type[] { propertyInfo.PropertyType });
//						ilGenerator = setMethodBuilder.GetILGenerator();
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldarg, 1);
//						ilGenerator.Emit(OpCodes.Stfld, fieldBuilder);
//						ilGenerator.Emit(OpCodes.Ret);
//						_intermediateImplementationTypeBuilder.DefineMethodOverride(setMethodBuilder, propertyInfo.GetSetMethod());
//					}
//					_intermediateImplementationTypeBuilder.CreateType();
//				}

//				internal override sealed void PostCompile()
//				{
//					base.PostCompile();
//					int referenceSinglePropertyCount = _descriptorTypeReferenceSinglePropertyCompilers.Length;
//					for (int singleReferencePropertyIndex = 0; singleReferencePropertyIndex < referenceSinglePropertyCount; singleReferencePropertyIndex++)
//						_descriptorTypeReferenceSinglePropertyCompilers[singleReferencePropertyIndex]._setMethodBuilder.GetILGenerator().ThrowException(_invalidOperationExceptionType);
//					_descriptorTypeBuilder.CreateType();
//					_implementationTypeBuilder.CreateType();
//				}
//			}
//			private sealed class ActualVersionCompiler : VersionCompiler
//			{
//				internal readonly FieldBuilder _descriptorTypeDescriptorPointerManagerFieldBuilder;
//				internal readonly FieldBuilder _descriptorTypeUseCountFieldBuilder;
//				internal readonly ConstructorBuilder _descriptorTypeConstructorBuilder;

//				internal ActualVersionCompiler(ReferenceTypeCompiler referenceTypeCompiler, TypeVersionBuilderBase versionBuilder, int index) : base(referenceTypeCompiler, versionBuilder, index)
//				{
//					_descriptorTypeDescriptorPointerManagerFieldBuilder = _descriptorTypeBuilder.DefineField("_descriptorPointerManager", _pointerManagerType, FieldAttributes.PrivateScope);
//					_descriptorTypeUseCountFieldBuilder = _descriptorTypeBuilder.DefineField("_useCount", _int32Type, FieldAttributes.PrivateScope);
//					_descriptorTypeConstructorBuilder = _descriptorTypeBuilder.DefineConstructor(MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[] { _pointerManagerType, _objectManagerType });
//					ILGenerator ilGenerator = _descriptorTypeConstructorBuilder.GetILGenerator();
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Call, _objectConstructorInfo);
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Ldarg, 1);
//					ilGenerator.Emit(OpCodes.Stfld, _descriptorTypeDescriptorPointerManagerFieldBuilder);
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Ldarg, 2);
//					ilGenerator.Emit(OpCodes.Stfld, _descriptorTypeValueManagerFieldBuilder);
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Ldarg, 2);
//					ilGenerator.Emit(OpCodes.Call, _objectManagerTypeDataConstantLengthPropertyGetMethodInfo);
//					ilGenerator.Emit(OpCodes.Stfld, _descriptorTypeDataConstantLengthFieldBuilder);
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Ldc_I4, 1);
//					ilGenerator.Emit(OpCodes.Stfld, _descriptorTypeUseCountFieldBuilder);
//					ilGenerator.Emit(OpCodes.Ret);
//					int constantLengthPropertyCount = _descriptorTypeDataConstantLengthPropertyCompilers.Length;
//					for (int constantLengthPropertyIndex = 0; constantLengthPropertyIndex < constantLengthPropertyCount; constantLengthPropertyIndex++)
//					{
//						ilGenerator = _descriptorTypeDataConstantLengthPropertyCompilers[constantLengthPropertyIndex]._setMethodBuilder.GetILGenerator();
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldfld, _descriptorTypeValueManagerFieldBuilder);
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Dup);
//						ilGenerator.Emit(OpCodes.Ldflda, _descriptorTypeDataConstantLengthFieldBuilder);
//						ilGenerator.Emit(OpCodes.Ldarg, 1);
//						ilGenerator.Emit(OpCodes.Stfld, _dataConstantLengthCompiler._fieldBuilders[constantLengthPropertyIndex]);
//						ilGenerator.Emit(OpCodes.Ldfld, _descriptorTypeDataConstantLengthFieldBuilder);
//						ilGenerator.Emit(OpCodes.Call, _objectManagerTypeDataConstantLengthPropertySetMethodInfo);
//						ilGenerator.Emit(OpCodes.Ret);
//					}
//					int variableLengthPropertyCount = _descriptorTypeDataVariableLengthPropertyCompilers.Length;
//					for (int variableLengthPropertyIndex = 0; variableLengthPropertyIndex < variableLengthPropertyCount; variableLengthPropertyIndex++)
//					{
//						ilGenerator = _descriptorTypeDataVariableLengthPropertyCompilers[variableLengthPropertyIndex]._setMethodBuilder.GetILGenerator();
//						Label returnLabel = ilGenerator.DefineLabel();
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldfld, _descriptorTypeValueManagerFieldBuilder);
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Dup);
//						ilGenerator.Emit(OpCodes.Dup);
//						ilGenerator.Emit(OpCodes.Ldfld, _descriptorTypeDataVariableLengthLoadedFieldBuilder);
//						ilGenerator.Emit(OpCodes.Brtrue, returnLabel);
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Call, _descriptorTypeLoadDataVariableLengthMethodBuilder);
//						ilGenerator.MarkLabel(returnLabel);
//						ilGenerator.Emit(OpCodes.Ldflda, _descriptorTypeDataVariableLengthFieldBuilder);
//						ilGenerator.Emit(OpCodes.Ldarg, 1);
//						ilGenerator.Emit(OpCodes.Stfld, _dataVariableLengthCompiler._fieldBuilders[variableLengthPropertyIndex]);
//						ilGenerator.Emit(OpCodes.Ldfld, _descriptorTypeDataVariableLengthFieldBuilder);
//						ilGenerator.Emit(OpCodes.Call, _objectManagerTypeDataVariableLengthPropertySetMethodInfo);
//						ilGenerator.Emit(OpCodes.Ret);
//					}
//				}

//				internal override sealed void PostCompile()
//				{
//					base.PostCompile();
//					ILGenerator ilGenerator;
//					int singleReferencePropertyCount = _descriptorTypeReferenceSinglePropertyCompilers.Length;
//					for (int singleReferencePropertyIndex = 0; singleReferencePropertyIndex < singleReferencePropertyCount; singleReferencePropertyIndex++)
//					{
//						DescriptorReferenceCompiler referenceCompiler = _descriptorTypeReferenceSingleCompilers[singleReferencePropertyIndex];
//						ReferenceTypeCompiler referenceTypeCompiler = _referenceTypeCompiler._modelCompiler._referenceTypeCompilers[referenceCompiler._fieldBuilder.FieldType];
//						ilGenerator = _descriptorTypeReferenceSinglePropertyCompilers[singleReferencePropertyIndex]._setMethodBuilder.GetILGenerator();
//						Label chooseNullLabel = ilGenerator.DefineLabel();
//						Label setLabel = ilGenerator.DefineLabel();
//						Label markLabel = ilGenerator.DefineLabel();
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Dup);
//						ilGenerator.Emit(OpCodes.Dup);
//						ilGenerator.Emit(OpCodes.Ldarg, 1);
//						ilGenerator.Emit(OpCodes.Stfld, referenceCompiler._fieldBuilder);
//						ilGenerator.Emit(OpCodes.Ldfld, _descriptorTypeValueManagerFieldBuilder);
//						ilGenerator.Emit(OpCodes.Ldc_I4, referenceCompiler._pointerIndex);
//						ilGenerator.Emit(OpCodes.Ldarg, 1);
//						ilGenerator.Emit(OpCodes.Brfalse, chooseNullLabel);
//						ilGenerator.Emit(OpCodes.Ldarg, 1);
//						ilGenerator.Emit(OpCodes.Ldfld, referenceTypeCompiler._actualVersionCompiler._implementationTypeDescriptorFieldBuilder);
//						ilGenerator.Emit(OpCodes.Ldfld, referenceTypeCompiler._actualVersionCompiler._descriptorTypeDescriptorPointerManagerFieldBuilder);
//						ilGenerator.Emit(OpCodes.Br, setLabel);
//						ilGenerator.MarkLabel(chooseNullLabel);
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldfld, _descriptorTypeDescriptorPointerManagerFieldBuilder);
//						ilGenerator.Emit(OpCodes.Call, _pointerManagerTypeDomainPropertyGetMethodInfo);
//						ilGenerator.Emit(OpCodes.Call, _domainTypeNullPointerGetMethodInfo);
//						ilGenerator.MarkLabel(setLabel);
//						ilGenerator.Emit(OpCodes.Call, _objectManagerTypeItemPropertySetMethodInfo);
//						ilGenerator.Emit(OpCodes.Ldfld, referenceCompiler._loadedFieldBuilder);
//						ilGenerator.Emit(OpCodes.Brfalse, markLabel);
//						ilGenerator.Emit(OpCodes.Ret);
//						ilGenerator.MarkLabel(markLabel);
//						ilGenerator.Emit(OpCodes.Ldarg, 0);
//						ilGenerator.Emit(OpCodes.Ldc_I4, 1);
//						ilGenerator.Emit(OpCodes.Stfld, referenceCompiler._loadedFieldBuilder);
//						ilGenerator.Emit(OpCodes.Ret);
//					}
//					_descriptorTypeBuilder.CreateType();
//					MethodInfo finalizeMethodInfo = _implementationTypeBuilder.BaseType.GetMethod("Finalize", BindingFlags.Instance | BindingFlags.NonPublic);
//					MethodBuilder finalizeMethodBuilder = _implementationTypeBuilder.DefineMethod(finalizeMethodInfo.Name, MethodAttributes.Family | MethodAttributes.HideBySig | MethodAttributes.Virtual, _voidType, Type.EmptyTypes);
//					ilGenerator = finalizeMethodBuilder.GetILGenerator();
//					ilGenerator.BeginExceptionBlock();
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Ldfld, _implementationTypeDescriptorFieldBuilder);
//					ilGenerator.Emit(OpCodes.Call, _referenceTypeCompiler._releaseMethodBuilder);
//					ilGenerator.BeginFinallyBlock();
//					ilGenerator.Emit(OpCodes.Ldarg, 0);
//					ilGenerator.Emit(OpCodes.Call, finalizeMethodInfo);
//					ilGenerator.EndExceptionBlock();
//					ilGenerator.Emit(OpCodes.Ret);
//					_implementationTypeBuilder.DefineMethodOverride(finalizeMethodBuilder, finalizeMethodInfo);
//					_implementationTypeBuilder.CreateType();
//				}
//			}

//			private readonly ModelCompiler _modelCompiler;
//			private readonly TypeBuilder _typeBuilder;
//			private readonly ObsoleteVersionCompiler[] _obsoleteVersionCompilers;
//			private readonly ActualVersionCompiler _actualVersionCompiler;
//			private readonly ConstructorInfo _dictionaryTypeConstructorInfo;
//			private readonly MethodInfo _dictionaryTypeAddMethodInfo;
//			private readonly MethodInfo _dictionaryTypeTryGetValueMethodInfo;
//			private readonly MethodInfo _dictionaryTypeRemoveMethodInfo;
//			private readonly TypeBuilder _activatorTypeBuilder;
//			private readonly FieldBuilder _descriptorDictionaryFieldBuilder;
//			private readonly MethodBuilder _createMethodBuilder;
//			private readonly MethodBuilder _openMethodBuilder;
//			private readonly MethodBuilder _releaseMethodBuilder;
//			internal SingleCreator _singleCreator;
//			internal SingleOpener _singleOpener;

//			internal ReferenceTypeCompiler(ModelCompiler modelCompiler, DatabaseTypeBuilder referenceTypeBuilder)
//			{
//				_modelCompiler = modelCompiler;
//				(_typeBuilder = modelCompiler._moduleBuilder.DefineType(referenceTypeBuilder._name, TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed)).CreateType();
//				int lastVersionCompilerIndex = referenceTypeBuilder._versionBuilders.Count - 1;
//				_obsoleteVersionCompilers = new ObsoleteVersionCompiler[lastVersionCompilerIndex];
//				for (int versionCompilerIndex = 0; versionCompilerIndex < lastVersionCompilerIndex; versionCompilerIndex++)
//					_obsoleteVersionCompilers[versionCompilerIndex] = new ObsoleteVersionCompiler(this, referenceTypeBuilder._versionBuilders[versionCompilerIndex], versionCompilerIndex);
//				_actualVersionCompiler = new ActualVersionCompiler(this, referenceTypeBuilder._versionBuilders[lastVersionCompilerIndex], lastVersionCompilerIndex);
//				Type dictionaryType = _dictionaryTypeDefinition.MakeGenericType(_pointerManagerType, _actualVersionCompiler._descriptorTypeBuilder);
//				_dictionaryTypeConstructorInfo = TypeBuilder.GetConstructor(dictionaryType, _dictionaryTypeDefinitionConstructorInfo);
//				_dictionaryTypeAddMethodInfo = TypeBuilder.GetMethod(dictionaryType, _dictionaryTypeDefinitionAddMethodInfo);
//				_dictionaryTypeTryGetValueMethodInfo = TypeBuilder.GetMethod(dictionaryType, _dictionaryTypeDefinitionTryGetValueMethodInfo);
//				_dictionaryTypeRemoveMethodInfo = TypeBuilder.GetMethod(dictionaryType, _dictionaryTypeDefinitionRemoveMethodInfo);
//				_activatorTypeBuilder = _typeBuilder.DefineNestedType("Activator", TypeAttributes.NestedPublic | TypeAttributes.Abstract | TypeAttributes.Sealed);
//				_descriptorDictionaryFieldBuilder = _activatorTypeBuilder.DefineField("_descriptorDictionary", dictionaryType, FieldAttributes.Static | FieldAttributes.Private);
//				_createMethodBuilder = _activatorTypeBuilder.DefineMethod("Create", MethodAttributes.Static | MethodAttributes.Public | MethodAttributes.HideBySig, _objectType, new Type[] { _domainManagerType, _pointerManagerByRefType });
//				_openMethodBuilder = _activatorTypeBuilder.DefineMethod("Open", MethodAttributes.Static | MethodAttributes.Public | MethodAttributes.HideBySig, _objectType, new Type[] { _pointerManagerType });
//				_releaseMethodBuilder = _activatorTypeBuilder.DefineMethod("Release", MethodAttributes.Static | MethodAttributes.HideBySig, _voidType, new Type[] { _actualVersionCompiler._descriptorTypeBuilder });
//			}

//			internal void PostCompile()
//			{
//				foreach (ObsoleteVersionCompiler versionCompiler in _obsoleteVersionCompilers)
//					versionCompiler.PostCompile();
//				_actualVersionCompiler.PostCompile();
//				int lastVersionCompilerIndex = _obsoleteVersionCompilers.Length;
//				Type func2Type = _func2TypeDefinition.MakeGenericType(_descriptorValueManagerType, _actualVersionCompiler._implementationTypeBuilder);
//				ConstructorInfo func2TypeConstructorInfo = TypeBuilder.GetConstructor(func2Type, _func2TypeDefinitionConstructorInfo);
//				MethodInfo func2TypeInvokeMethodInfo = TypeBuilder.GetMethod(func2Type, _func2TypeDefinitionInvokeMethodInfo);
//				FieldBuilder translatorsFieldInfo = _activatorTypeBuilder.DefineField("_translators", func2Type.MakeArrayType(), FieldAttributes.Static | FieldAttributes.Private);
//				ConstructorBuilder сonstructorBuilder = _activatorTypeBuilder.DefineConstructor(MethodAttributes.Static | MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, Type.EmptyTypes);
//				ILGenerator ilGenerator = сonstructorBuilder.GetILGenerator();
//				ilGenerator.Emit(OpCodes.Ldc_I4, lastVersionCompilerIndex + 1);
//				ilGenerator.Emit(OpCodes.Newarr, func2Type);
//				ilGenerator.Emit(OpCodes.Stsfld, translatorsFieldInfo);
//				ilGenerator.Emit(OpCodes.Newobj, _dictionaryTypeConstructorInfo);
//				ilGenerator.Emit(OpCodes.Stsfld, _descriptorDictionaryFieldBuilder);
//				MethodBuilder translateMethodBuilder;
//				ILGenerator translateMethodILGenerator;
//				for (int versionCompilerIndex = 0; versionCompilerIndex < lastVersionCompilerIndex; versionCompilerIndex++)
//				{
//					int translationVersionCompilerIndex = versionCompilerIndex;
//					ObsoleteVersionCompiler obsoleteVersionCompiler = _obsoleteVersionCompilers[translationVersionCompilerIndex];
//					translateMethodBuilder = _activatorTypeBuilder.DefineMethod("Translate_" + translationVersionCompilerIndex.ToString(), MethodAttributes.Static | MethodAttributes.Private | MethodAttributes.HideBySig, _actualVersionCompiler._implementationTypeBuilder, new Type[] { _descriptorValueManagerType });
//					ilGenerator.Emit(OpCodes.Ldsfld, translatorsFieldInfo);
//					ilGenerator.Emit(OpCodes.Ldc_I4, translationVersionCompilerIndex);
//					ilGenerator.Emit(OpCodes.Ldnull);
//					ilGenerator.Emit(OpCodes.Ldftn, translateMethodBuilder);
//					ilGenerator.Emit(OpCodes.Newobj, func2TypeConstructorInfo);
//					ilGenerator.Emit(OpCodes.Stelem_Ref);
//					translateMethodILGenerator = translateMethodBuilder.GetILGenerator();
//					translateMethodILGenerator.DeclareLocal(_pointerManagerType);
//					translateMethodILGenerator.DeclareLocal(_objectType);
//					translateMethodILGenerator.DeclareLocal(_actualVersionCompiler._objectManagerType);
//					translateMethodILGenerator.DeclareLocal(_actualVersionCompiler._descriptorTypeBuilder);
//					translateMethodILGenerator.Emit(OpCodes.Ldarg, 0);
//					translateMethodILGenerator.Emit(OpCodes.Dup);
//					translateMethodILGenerator.Emit(OpCodes.Call, _descriptorValueManagerTypePointerPropertyGetMethodInfo);
//					translateMethodILGenerator.Emit(OpCodes.Stloc, 0);
//					translateMethodILGenerator.Emit(OpCodes.Ldc_I4, 0);
//					translateMethodILGenerator.Emit(OpCodes.Call, _descriptorValueManagerTypeItemPropertyGetMethodInfo);
//					translateMethodILGenerator.Emit(OpCodes.Ldsfld, obsoleteVersionCompiler._dataConstantLengthCompiler._bitConverterBuilderTypeInstanceFieldBuilder);
//					translateMethodILGenerator.Emit(OpCodes.Ldsfld, obsoleteVersionCompiler._dataVariableLengthCompiler._bitConverterBuilderTypeInstanceFieldBuilder);
//					translateMethodILGenerator.Emit(OpCodes.Call, obsoleteVersionCompiler._objectManagerActivatorTypeOpenMethodInfo);
//					translateMethodILGenerator.Emit(OpCodes.Newobj, obsoleteVersionCompiler._descriptorTypeConstructorBuilder);
//					translateMethodILGenerator.Emit(OpCodes.Newobj, obsoleteVersionCompiler._implementationTypeConstructorBuilder);
//					translateMethodILGenerator.Emit(OpCodes.Stloc, 1);
//					for (translationVersionCompilerIndex++; translationVersionCompilerIndex < lastVersionCompilerIndex; translationVersionCompilerIndex++)
//					{
//						obsoleteVersionCompiler = _obsoleteVersionCompilers[translationVersionCompilerIndex];
//						translateMethodILGenerator.Emit(OpCodes.Ldloc, 1);
//						translateMethodILGenerator.Emit(OpCodes.Newobj, obsoleteVersionCompiler._intermediateImplementationTypeConstructorBuilder);
//						translateMethodILGenerator.Emit(OpCodes.Dup);
//						translateMethodILGenerator.Emit(OpCodes.Stloc, 1);
//						translateMethodILGenerator.Emit(OpCodes.Call, obsoleteVersionCompiler._translateMethodInfo);
//					}
//					translateMethodILGenerator.Emit(OpCodes.Ldloc, 1);
//					translateMethodILGenerator.Emit(OpCodes.Ldloc, 0);
//					translateMethodILGenerator.Emit(OpCodes.Dup);
//					translateMethodILGenerator.Emit(OpCodes.Call, _pointerManagerTypeDomainPropertyGetMethodInfo);
//					translateMethodILGenerator.Emit(OpCodes.Ldsfld, _actualVersionCompiler._dataConstantLengthCompiler._bitConverterBuilderTypeInstanceFieldBuilder);
//					translateMethodILGenerator.Emit(OpCodes.Ldsfld, _actualVersionCompiler._dataVariableLengthCompiler._bitConverterBuilderTypeInstanceFieldBuilder);
//					translateMethodILGenerator.Emit(OpCodes.Ldc_I4, _actualVersionCompiler._referenceCount);
//					translateMethodILGenerator.Emit(OpCodes.Call, _actualVersionCompiler._objectManagerActivatorTypeCreateMethodInfo);
//					translateMethodILGenerator.Emit(OpCodes.Dup);
//					translateMethodILGenerator.Emit(OpCodes.Stloc, 2);
//					translateMethodILGenerator.Emit(OpCodes.Newobj, _actualVersionCompiler._descriptorTypeConstructorBuilder);
//					translateMethodILGenerator.Emit(OpCodes.Dup);
//					translateMethodILGenerator.Emit(OpCodes.Stloc, 3);
//					translateMethodILGenerator.Emit(OpCodes.Newobj, _actualVersionCompiler._implementationTypeConstructorBuilder);
//					translateMethodILGenerator.Emit(OpCodes.Dup);
//					translateMethodILGenerator.Emit(OpCodes.Stloc, 1);
//					translateMethodILGenerator.Emit(OpCodes.Call, _actualVersionCompiler._translateMethodInfo);

//					translateMethodILGenerator.Emit(OpCodes.Ldarg, 0);
//					translateMethodILGenerator.Emit(OpCodes.Ldc_I4, lastVersionCompilerIndex);
//					translateMethodILGenerator.Emit(OpCodes.Call, _descriptorValueManagerTypeDataConstantLengthPropertySetMethodInfo);

//					translateMethodILGenerator.Emit(OpCodes.Ldarg, 0);
//					translateMethodILGenerator.Emit(OpCodes.Ldc_I4, 0);
//					translateMethodILGenerator.Emit(OpCodes.Ldloc, 2);
//					translateMethodILGenerator.Emit(OpCodes.Call, _actualVersionCompiler._objectManagerTypePointerPropertyGetMethodInfo);
//					translateMethodILGenerator.Emit(OpCodes.Call, _descriptorValueManagerTypeItemPropertySetMethodInfo);

//					translateMethodILGenerator.Emit(OpCodes.Ldsfld, _descriptorDictionaryFieldBuilder);
//					translateMethodILGenerator.Emit(OpCodes.Ldloc, 0);
//					translateMethodILGenerator.Emit(OpCodes.Ldloc, 3);
//					translateMethodILGenerator.Emit(OpCodes.Call, _dictionaryTypeAddMethodInfo);

//					translateMethodILGenerator.Emit(OpCodes.Ldloc, 1);
//					translateMethodILGenerator.Emit(OpCodes.Ret);
//				}
//				translateMethodBuilder = _activatorTypeBuilder.DefineMethod("Translate_" + lastVersionCompilerIndex.ToString(), MethodAttributes.Static | MethodAttributes.Private | MethodAttributes.HideBySig, _actualVersionCompiler._implementationTypeBuilder, new Type[] { _descriptorValueManagerType });
//				ilGenerator.Emit(OpCodes.Ldsfld, translatorsFieldInfo);
//				ilGenerator.Emit(OpCodes.Ldc_I4, lastVersionCompilerIndex);
//				ilGenerator.Emit(OpCodes.Ldnull);
//				ilGenerator.Emit(OpCodes.Ldftn, translateMethodBuilder);
//				ilGenerator.Emit(OpCodes.Newobj, func2TypeConstructorInfo);
//				ilGenerator.Emit(OpCodes.Stelem_Ref);
//				ilGenerator.Emit(OpCodes.Ret);
//				translateMethodILGenerator = translateMethodBuilder.GetILGenerator();
//				translateMethodILGenerator.DeclareLocal(_pointerManagerType);
//				translateMethodILGenerator.DeclareLocal(_actualVersionCompiler._descriptorTypeBuilder);
//				translateMethodILGenerator.Emit(OpCodes.Ldarg, 0);
//				translateMethodILGenerator.Emit(OpCodes.Call, _descriptorValueManagerTypePointerPropertyGetMethodInfo);
//				translateMethodILGenerator.Emit(OpCodes.Dup);
//				translateMethodILGenerator.Emit(OpCodes.Stloc, 0);
//				translateMethodILGenerator.Emit(OpCodes.Ldarg, 0);
//				translateMethodILGenerator.Emit(OpCodes.Ldc_I4, 0);
//				translateMethodILGenerator.Emit(OpCodes.Call, _descriptorValueManagerTypeItemPropertyGetMethodInfo);
//				translateMethodILGenerator.Emit(OpCodes.Ldsfld, _actualVersionCompiler._dataConstantLengthCompiler._bitConverterBuilderTypeInstanceFieldBuilder);
//				translateMethodILGenerator.Emit(OpCodes.Ldsfld, _actualVersionCompiler._dataVariableLengthCompiler._bitConverterBuilderTypeInstanceFieldBuilder);
//				translateMethodILGenerator.Emit(OpCodes.Call, _actualVersionCompiler._objectManagerActivatorTypeOpenMethodInfo);
//				translateMethodILGenerator.Emit(OpCodes.Newobj, _actualVersionCompiler._descriptorTypeConstructorBuilder);
//				translateMethodILGenerator.Emit(OpCodes.Dup);
//				translateMethodILGenerator.Emit(OpCodes.Stloc, 1);
//				translateMethodILGenerator.Emit(OpCodes.Newobj, _actualVersionCompiler._implementationTypeConstructorBuilder);

//				translateMethodILGenerator.Emit(OpCodes.Ldsfld, _descriptorDictionaryFieldBuilder);
//				translateMethodILGenerator.Emit(OpCodes.Ldloc, 0);
//				translateMethodILGenerator.Emit(OpCodes.Ldloc, 1);
//				translateMethodILGenerator.Emit(OpCodes.Call, _dictionaryTypeAddMethodInfo);

//				translateMethodILGenerator.Emit(OpCodes.Ret);

//				ilGenerator = _createMethodBuilder.GetILGenerator();
//				ilGenerator.DeclareLocal(_descriptorValueManagerType);
//				ilGenerator.DeclareLocal(_actualVersionCompiler._objectManagerType);
//				ilGenerator.DeclareLocal(_actualVersionCompiler._descriptorTypeBuilder);

//				ilGenerator.Emit(OpCodes.Ldarg, 1);
//				ilGenerator.Emit(OpCodes.Ldarg, 0);
//				ilGenerator.Emit(OpCodes.Ldsfld, _int32BitConverterBuilderTypeInstanceFieldInfo);
//				ilGenerator.Emit(OpCodes.Ldsfld, _emptyDataTypeVariableLengthBitConverterInstanceFieldInfo);
//				ilGenerator.Emit(OpCodes.Ldc_I4, 1);
//				ilGenerator.Emit(OpCodes.Call, _descriptorValueManagerActivatorTypeCreateMethodInfo);
//				ilGenerator.Emit(OpCodes.Dup);
//				ilGenerator.Emit(OpCodes.Stloc, 0);
//				ilGenerator.Emit(OpCodes.Call, _descriptorValueManagerTypePointerPropertyGetMethodInfo);
//				ilGenerator.Emit(OpCodes.Stind_Ref);

//				ilGenerator.Emit(OpCodes.Ldloc, 0);
//				ilGenerator.Emit(OpCodes.Ldc_I4, lastVersionCompilerIndex);
//				ilGenerator.Emit(OpCodes.Call, _descriptorValueManagerTypeDataConstantLengthPropertySetMethodInfo);

//				ilGenerator.Emit(OpCodes.Ldarg, 0);
//				ilGenerator.Emit(OpCodes.Ldsfld, _actualVersionCompiler._dataConstantLengthCompiler._bitConverterBuilderTypeInstanceFieldBuilder);
//				ilGenerator.Emit(OpCodes.Ldsfld, _actualVersionCompiler._dataVariableLengthCompiler._bitConverterBuilderTypeInstanceFieldBuilder);
//				ilGenerator.Emit(OpCodes.Ldc_I4, _actualVersionCompiler._referenceCount);
//				ilGenerator.Emit(OpCodes.Call, _actualVersionCompiler._objectManagerActivatorTypeCreateMethodInfo);
//				ilGenerator.Emit(OpCodes.Stloc, 1);

//				ilGenerator.Emit(OpCodes.Ldloc, 0);
//				ilGenerator.Emit(OpCodes.Ldc_I4, 0);
//				ilGenerator.Emit(OpCodes.Ldloc, 1);
//				ilGenerator.Emit(OpCodes.Call, _actualVersionCompiler._objectManagerTypePointerPropertyGetMethodInfo);
//				ilGenerator.Emit(OpCodes.Call, _descriptorValueManagerTypeItemPropertySetMethodInfo);

//				ilGenerator.Emit(OpCodes.Ldarg, 1);
//				ilGenerator.Emit(OpCodes.Ldind_Ref);
//				ilGenerator.Emit(OpCodes.Ldloc, 1);
//				ilGenerator.Emit(OpCodes.Newobj, _actualVersionCompiler._descriptorTypeConstructorBuilder);
//				ilGenerator.Emit(OpCodes.Stloc, 2);

//				ilGenerator.Emit(OpCodes.Ldsfld, _descriptorDictionaryFieldBuilder);
//				ilGenerator.Emit(OpCodes.Ldarg, 1);
//				ilGenerator.Emit(OpCodes.Ldind_Ref);
//				ilGenerator.Emit(OpCodes.Ldloc, 2);
//				ilGenerator.Emit(OpCodes.Call, _dictionaryTypeAddMethodInfo);

//				ilGenerator.Emit(OpCodes.Ldloc, 2);
//				ilGenerator.Emit(OpCodes.Newobj, _actualVersionCompiler._implementationTypeConstructorBuilder);
//				ilGenerator.Emit(OpCodes.Ret);

//				ilGenerator = _openMethodBuilder.GetILGenerator();
//				Label readLabel = ilGenerator.DefineLabel();
//				ilGenerator.DeclareLocal(_descriptorValueManagerType);
//				ilGenerator.DeclareLocal(_actualVersionCompiler._descriptorTypeBuilder);
//				ilGenerator.DeclareLocal(_actualVersionCompiler._implementationTypeBuilder);

//				ilGenerator.Emit(OpCodes.Ldsfld, _descriptorDictionaryFieldBuilder);
//				ilGenerator.Emit(OpCodes.Ldarg, 0);
//				ilGenerator.Emit(OpCodes.Ldloca, 1);
//				ilGenerator.Emit(OpCodes.Call, _dictionaryTypeTryGetValueMethodInfo);
//				ilGenerator.Emit(OpCodes.Brfalse, readLabel);
//				ilGenerator.Emit(OpCodes.Ldloc, 1);
//				ilGenerator.Emit(OpCodes.Dup);
//				ilGenerator.Emit(OpCodes.Dup);
//				ilGenerator.Emit(OpCodes.Ldfld, _actualVersionCompiler._descriptorTypeUseCountFieldBuilder);
//				ilGenerator.Emit(OpCodes.Ldc_I4, 1);
//				ilGenerator.Emit(OpCodes.Add_Ovf);
//				ilGenerator.Emit(OpCodes.Stfld, _actualVersionCompiler._descriptorTypeUseCountFieldBuilder);
//				ilGenerator.Emit(OpCodes.Newobj, _actualVersionCompiler._implementationTypeConstructorBuilder);
//				ilGenerator.Emit(OpCodes.Ret);
//				ilGenerator.MarkLabel(readLabel);

//				ilGenerator.Emit(OpCodes.Ldarg, 0);
//				ilGenerator.Emit(OpCodes.Ldsfld, _int32BitConverterBuilderTypeInstanceFieldInfo);
//				ilGenerator.Emit(OpCodes.Ldsfld, _emptyDataTypeVariableLengthBitConverterInstanceFieldInfo);
//				ilGenerator.Emit(OpCodes.Call, _descriptorValueManagerActivatorTypeOpenMethodInfo);
//				ilGenerator.Emit(OpCodes.Stloc, 0);

//				ilGenerator.Emit(OpCodes.Ldsfld, translatorsFieldInfo);
//				ilGenerator.Emit(OpCodes.Ldloc, 0);
//				ilGenerator.Emit(OpCodes.Call, _descriptorValueManagerTypeDataConstantLengthPropertyGetMethodInfo);
//				ilGenerator.Emit(OpCodes.Ldelem_Ref);
//				ilGenerator.Emit(OpCodes.Ldloc, 0);
//				ilGenerator.Emit(OpCodes.Call, func2TypeInvokeMethodInfo);
//				ilGenerator.Emit(OpCodes.Ret);

//				ilGenerator = _releaseMethodBuilder.GetILGenerator();
//				ilGenerator.DeclareLocal(_booleanType);
//				Label returnLabel = ilGenerator.DefineLabel();
//				ilGenerator.Emit(OpCodes.Ldarg, 0);
//				ilGenerator.Emit(OpCodes.Dup);
//				ilGenerator.Emit(OpCodes.Ldfld, _actualVersionCompiler._descriptorTypeUseCountFieldBuilder);
//				ilGenerator.Emit(OpCodes.Ldc_I4, 1);
//				ilGenerator.Emit(OpCodes.Sub);
//				ilGenerator.Emit(OpCodes.Dup);
//				ilGenerator.Emit(OpCodes.Ldc_I4, 0);
//				ilGenerator.Emit(OpCodes.Ceq);
//				ilGenerator.Emit(OpCodes.Stloc, 0);
//				ilGenerator.Emit(OpCodes.Stfld, _actualVersionCompiler._descriptorTypeUseCountFieldBuilder);

//				ilGenerator.Emit(OpCodes.Ldloc, 0);
//				ilGenerator.Emit(OpCodes.Brfalse, returnLabel);

//				ilGenerator.Emit(OpCodes.Ldsfld, _descriptorDictionaryFieldBuilder);
//				ilGenerator.Emit(OpCodes.Ldarg, 0);
//				ilGenerator.Emit(OpCodes.Ldfld, _actualVersionCompiler._descriptorTypeDescriptorPointerManagerFieldBuilder);
//				ilGenerator.Emit(OpCodes.Call, _dictionaryTypeRemoveMethodInfo);
//				ilGenerator.Emit(OpCodes.Pop);

//				ilGenerator.MarkLabel(returnLabel);
//				ilGenerator.Emit(OpCodes.Ret);

//				Type activatorType = _activatorTypeBuilder.CreateType();
//				_singleCreator = (SingleCreator)Delegate.CreateDelegate(_singleCreatorType, activatorType.GetMethod(_createMethodBuilder.Name));
//				_singleOpener = (SingleOpener)Delegate.CreateDelegate(_singleOpenerType, activatorType.GetMethod(_openMethodBuilder.Name));
//			}
//		}
//		//private sealed class ValueTypeCompiler
//		//{
//		//	internal sealed class VersionCompiler
//		//	{
//		//		internal sealed class FieldCompiler
//		//		{
//		//			private readonly List<FieldBuilder> _path;

//		//			internal FieldCompiler(List<FieldBuilder> path) => _path = path;
//		//		}

//		//		static private void Pass(List<FieldBuilder> path)
//		//		{

//		//		}

//		//		internal readonly List<FieldCompiler> _fieldCompilers;

//		//		internal VersionCompiler(ValueTypeVersionBuilderBase versionBuilder)
//		//		{

//		//		}
//		//	}

//		//	internal ValueTypeCompiler(ModelCompiler modelCompiler, ValueTypeBuilder valueTypeBuilder)
//		//	{

//		//	}
//		//}

//		private delegate object SingleCreator(DomainManager domainManager, out PointerManager pointerManager);
//		private delegate object SingleOpener(PointerManager pointerManager);

//		static private readonly Type _voidType;
//		static private readonly Type _objectType;
//		static private readonly Type _valueTypeType;
//		static private readonly Type _int32Type;
//		static private readonly Type _booleanType;
//		static private readonly Type _func2TypeDefinition;
//		static private readonly Type _dictionaryTypeDefinition;
//		static private readonly Type _fieldInfoType;
//		static private readonly Type _domainManagerType;
//		static private readonly Type _pointerManagerType;
//		static private readonly Type _pointerManagerByRefType;
//		static private readonly Type _objectManagerTypeDefinition;
//		static private readonly Type _objectManagerActivatorType;
//		static private readonly Type _descriptorValueManagerType;
//		static private readonly Type _singleCreatorType;
//		static private readonly Type _singleOpenerType;
//		static private readonly FieldInfo _int32BitConverterBuilderTypeInstanceFieldInfo;
//		static private readonly FieldInfo _emptyDataTypeVariableLengthBitConverterInstanceFieldInfo;
//		static private readonly ConstructorInfo _objectConstructorInfo;
//		static private readonly ConstructorInfo _func2TypeDefinitionConstructorInfo;
//		static private readonly ConstructorInfo _dictionaryTypeDefinitionConstructorInfo;
//		static private readonly MethodInfo _func2TypeDefinitionInvokeMethodInfo;
//		static private readonly MethodInfo _dictionaryTypeDefinitionAddMethodInfo;
//		static private readonly MethodInfo _dictionaryTypeDefinitionTryGetValueMethodInfo;
//		static private readonly MethodInfo _dictionaryTypeDefinitionRemoveMethodInfo;
//		static private readonly MethodInfo _domainTypeNullPointerGetMethodInfo;
//		static private readonly MethodInfo _pointerManagerTypeDomainPropertyGetMethodInfo;
//		static private readonly MethodInfo _pointerManagerTypeIsNullPropertyGetMethodInfo;
//		static private readonly MethodInfo _descriptorValueManagerActivatorTypeCreateMethodInfo;
//		static private readonly MethodInfo _descriptorValueManagerActivatorTypeOpenMethodInfo;
//		static private readonly MethodInfo _descriptorValueManagerTypePointerPropertyGetMethodInfo;
//		static private readonly MethodInfo _descriptorValueManagerTypeDataConstantLengthPropertyGetMethodInfo;
//		static private readonly MethodInfo _descriptorValueManagerTypeDataConstantLengthPropertySetMethodInfo;
//		static private readonly MethodInfo _descriptorValueManagerTypeItemPropertyGetMethodInfo;
//		static private readonly MethodInfo _descriptorValueManagerTypeItemPropertySetMethodInfo;

//		static ModelCompiler()
//		{
//			_voidType = typeof(void);
//			_objectType = typeof(object);
//			_valueTypeType = typeof(ValueType);
//			_booleanType = typeof(bool);
//			_int32Type = typeof(int);
//			_fieldInfoType = typeof(FieldInfo);
//			_func2TypeDefinition = typeof(Func<,>);
//			_dictionaryTypeDefinition = typeof(Dictionary<,>);
//			_domainManagerType = typeof(DomainManager);
//			_pointerManagerType = typeof(PointerManager);
//			_pointerManagerByRefType = _pointerManagerType.MakeByRefType();
//			_objectManagerTypeDefinition = typeof(ValueManager<,>);
//			_objectManagerActivatorType = typeof(ValueManagerActivator);
//			Type emptyDataType = typeof(EmptyData);
//			Type[] _descriptorTypeArguments = new Type[] { _int32Type, emptyDataType };
//			_descriptorValueManagerType = _objectManagerTypeDefinition.MakeGenericType(_descriptorTypeArguments);
//			_singleCreatorType = typeof(SingleCreator);
//			_singleOpenerType = typeof(SingleOpener);
//			_int32BitConverterBuilderTypeInstanceFieldInfo = typeof(Int32BitConverterBuilder).GetField(nameof(Int32BitConverterBuilder.Instance));
//			_emptyDataTypeVariableLengthBitConverterInstanceFieldInfo = emptyDataType.GetField(nameof(EmptyData.VariableLengthBitConverterInstance));
//			_objectConstructorInfo = _objectType.GetConstructor(Type.EmptyTypes);
//			_func2TypeDefinitionConstructorInfo = _func2TypeDefinition.GetConstructors()[0];
//			_dictionaryTypeDefinitionConstructorInfo = _dictionaryTypeDefinition.GetConstructor(Type.EmptyTypes);
//			_func2TypeDefinitionInvokeMethodInfo = _func2TypeDefinition.GetMethod("Invoke");
//			_dictionaryTypeDefinitionAddMethodInfo = _dictionaryTypeDefinition.GetMethod(nameof(Dictionary<byte, byte>.Add));
//			_dictionaryTypeDefinitionTryGetValueMethodInfo = _dictionaryTypeDefinition.GetMethod(nameof(Dictionary<byte, byte>.TryGetValue));
//			_dictionaryTypeDefinitionRemoveMethodInfo = _dictionaryTypeDefinition.GetMethod(nameof(Dictionary<byte, byte>.Remove));
//			_domainTypeNullPointerGetMethodInfo = typeof(DomainManager).GetProperty(nameof(DomainManager.NullPointer)).GetGetMethod();
//			_pointerManagerTypeDomainPropertyGetMethodInfo = _pointerManagerType.GetProperty(nameof(PointerManager.Domain)).GetGetMethod();
//			_pointerManagerTypeIsNullPropertyGetMethodInfo = _pointerManagerType.GetProperty(nameof(PointerManager.IsNull)).GetGetMethod();
//			_descriptorValueManagerActivatorTypeCreateMethodInfo = _objectManagerActivatorType.GetMethod(nameof(ValueManagerActivator.Create)).MakeGenericMethod(_descriptorTypeArguments);
//			_descriptorValueManagerActivatorTypeOpenMethodInfo = _objectManagerActivatorType.GetMethod(nameof(ValueManagerActivator.Open)).MakeGenericMethod(_descriptorTypeArguments);
//			_descriptorValueManagerTypePointerPropertyGetMethodInfo = _descriptorValueManagerType.GetProperty(nameof(ValueManager<byte, byte>.Pointer)).GetGetMethod();
//			PropertyInfo objectManagerTypeDataConstantLengthPropertyInfo = _descriptorValueManagerType.GetProperty(nameof(ValueManager<byte, byte>.DataConstantLength));
//			_descriptorValueManagerTypeDataConstantLengthPropertyGetMethodInfo = objectManagerTypeDataConstantLengthPropertyInfo.GetGetMethod();
//			_descriptorValueManagerTypeDataConstantLengthPropertySetMethodInfo = objectManagerTypeDataConstantLengthPropertyInfo.GetSetMethod();
//			PropertyInfo objectManagerTypeItemPropertyInfo = _descriptorValueManagerType.GetProperty("Item");
//			_descriptorValueManagerTypeItemPropertyGetMethodInfo = objectManagerTypeItemPropertyInfo.GetGetMethod();
//			_descriptorValueManagerTypeItemPropertySetMethodInfo = objectManagerTypeItemPropertyInfo.GetSetMethod();
//		}

//		private readonly ModuleBuilder _moduleBuilder;
//		private readonly Dictionary<Type, ReferenceTypeCompiler> _referenceTypeCompilers;

//		internal ModelCompiler(ModelBuilder modelBuilder)
//		{
//			AssemblyName assemblyName = new AssemblyName("DatabaseModel");
//			_moduleBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(assemblyName.FullName);
//			int referenceTypeCompilerCount = modelBuilder._typeBuilders.Count;
//			_referenceTypeCompilers = new Dictionary<Type, ReferenceTypeCompiler>(referenceTypeCompilerCount);
//			for (int referenceTypeCompilerIndex = 0; referenceTypeCompilerIndex < referenceTypeCompilerCount; referenceTypeCompilerIndex++)
//			{
//				DatabaseTypeBuilder referenceTypeBuilder = modelBuilder._typeBuilders[referenceTypeCompilerIndex];
//				_referenceTypeCompilers.Add(referenceTypeBuilder.LastVersionBuilder._versionType, new ReferenceTypeCompiler(this, referenceTypeBuilder));
//			}
//			foreach (ReferenceTypeCompiler referenceTypeCompiler in _referenceTypeCompilers.Values)
//				referenceTypeCompiler.PostCompile();
//		}

//		internal object CreateSingle(Type referenceLastVersionType, DomainManager domainManager, out PointerManager pointerManager) => _referenceTypeCompilers.TryGetValue(referenceLastVersionType, out ReferenceTypeCompiler referenceTypeCompiler) ? referenceTypeCompiler._singleCreator(domainManager, out pointerManager) : throw new ArgumentException(nameof(referenceLastVersionType));
//		internal object OpenSingle(Type referenceLastVersionType, PointerManager pointerManager) => _referenceTypeCompilers.TryGetValue(referenceLastVersionType, out ReferenceTypeCompiler referenceTypeCompiler) ? referenceTypeCompiler._singleOpener(pointerManager) : throw new ArgumentException(nameof(referenceLastVersionType));
//	}
//}