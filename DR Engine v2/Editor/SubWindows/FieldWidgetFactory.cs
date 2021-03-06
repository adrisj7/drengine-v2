using System;
using System.Reflection;
using DREngine.Editor.SubWindows.FieldWidgets;
using DREngine.ResourceLoading;
using GameEngine;
using GameEngine.Game.UI;
using Microsoft.Xna.Framework;

namespace DREngine.Editor.SubWindows
{
    public static class FieldWidgetFactory
    {
        public static IFieldWidget CreateField(DREditor editor, FieldInfo field)
        {
            var widget = GetNewField(editor, field);
            widget.InitializeField(field);
            return widget;
        }

        private static IFieldWidget GetNewField(DREditor editor, FieldInfo field)
        {
            var type = field.FieldType;

            // Check for OVERRIDES

            // Container attribute
            var fieldOverride = field.GetCustomAttribute<OverrideFieldAttribute>();
            if (fieldOverride != null) return fieldOverride.GetOverrideWidget(editor, field);

            // STRING FIELD
            if (IsType(type, typeof(string))) return new StringTextFieldWidget();
            // BOOL
            if (IsType(type, typeof(bool))) return new BoolField();

            // DEFAULT NUMBERS
            if (IsType(type, typeof(int))) return new IntegerTextFieldWidget();
            if (IsType(type, typeof(float))) return new FloatTextFieldWidget();

            // Enum
            if (IsType(type, typeof(Enum))) return new GenericEnumWidget();

            // Vector2
            if (IsType(type, typeof(Vector2))) return new Vector2Widget();

            // Misc simple types
            if (IsType(type, typeof(Rect))) return new RectFieldWidget();
            if (IsType(type, typeof(Margin))) return new MarginFieldWidget(editor);

            // MISC
            if (IsType(type, typeof(OverridablePath)))
            {
                var attrib = field.GetCustomAttribute<ResourceTypeAttribute>();
                if (attrib == null)
                    throw new InvalidOperationException($"You forgot to add an attribute for {type.Name}.");
                var subType = attrib.Type;
                return new OverridablePathFieldWidget(editor, subType);
            }


            return new UnknownFieldWidget($"Unsupported type: {type}.");
        }

        private static bool IsType(Type typeToCheck, Type type)
        {
            // type is the parent here
            return type.IsAssignableFrom(typeToCheck);
        }

        // ReSharper disable once UnusedMember.Local
        private static bool IsTypeGeneric(Type typeToCheck, Type generic)
        {
            try
            {
                return IsType(typeToCheck.GetGenericTypeDefinition(), generic);
            }
            catch (Exception)
            {
                // If this is not a generic it will throw an exception.
                // If that happens, we're OK since we know one thing!
                return false;
            }
        }
    }
}