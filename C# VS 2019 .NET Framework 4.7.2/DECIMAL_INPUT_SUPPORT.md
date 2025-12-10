# Decimal/Floating-Point Input Support

## Overview
This document describes the enhancements made to support robust decimal and floating-point numeric input in the Graphical 2D Frame Analysis application.

## Changes Made

### 1. New Helper Class: InputParsingHelpers.cs
A new utility class has been added to provide culture-tolerant parsing of numeric input. This class supports both dot (.) and comma (,) as decimal separators, accommodating different regional number formats.

**Key Methods:**
- `TryParseDouble(string s, out double value)` - Attempts parsing with multiple culture formats
- `ParseDoubleOrThrow(string s, string paramName = null)` - Parses or throws exception with clear error message
- `ParseDoubleOrDefault(string s, double defaultValue = 0.0)` - Parses or returns default value
- `FormatDouble(double value)` - Formats double using InvariantCulture for consistent file I/O
- `FormatDouble(double value, string format)` - Formats double with custom format

**Parsing Strategy:**
1. Try CurrentCulture (respects user's locale)
2. Try InvariantCulture (dot as decimal separator)
3. Try replacing comma with dot and parsing with InvariantCulture

### 2. Updated Numeric Parsing
Replaced unsafe parsing calls throughout the codebase:
- **Form1.cs**: 91 instances of `double.Parse` and `Convert.ToDouble` replaced
- **Forms2 directory**: 53 instances across multiple forms:
  - FBeamProp.cs (beam properties: Area, I, E)
  - FSupDisp.cs (support displacements)
  - FconcM.cs (concentrated moments)
  - FconcP.cs (concentrated point loads)
  - FUDL.cs (uniformly distributed loads)
  - FShowTable.cs
  - FSuppData.cs

**Note:** Integer parsing for counts (nodes, elements, indices) remains unchanged using `int.Parse` or `int.TryParse` as appropriate.

### 3. Updated UI Input Handlers
All TextBox KeyPress event handlers have been updated to accept both decimal separators:

**Previous behavior:** Only allowed dot (.) as decimal separator
```csharp
&& (e.KeyChar != '.')
if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
```

**New behavior:** Allows both dot (.) and comma (,)
```csharp
&& (e.KeyChar != '.') && (e.KeyChar != ',')
if ((e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1) ||
    (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1))
```

**Updated Forms:**
- FBeamProp.cs (3 textboxes for A, I, E)
- FNodal.cs (3 textboxes for nodal loads)
- FSupDisp.cs (2 textboxes for support displacements)
- FSuppData.cs
- FUDL.cs
- FconcM.cs (moment inputs)
- FconcP.cs (point load inputs)

### 4. Enhanced Error Handling
Forms now provide user-friendly validation messages:
- FBeamProp.cs: Validates Area, Iz, and E are non-zero with improved error messages
- FSupDisp.cs: Validates support displacement limits with proper parsing

### 5. Consistent Serialization
When saving numeric values to files or data structures:
- Use `InputParsingHelpers.FormatDouble()` to ensure InvariantCulture formatting
- Ensures dot (.) is always used in saved data for consistency
- Reading still supports both formats for user input

## Testing

### Manual Testing
A test class `TestParsingHelpers.cs` has been included to demonstrate the parsing functionality:
- Tests dot separator: "1.23", "123.456"
- Tests comma separator: "1,23", "123,456"
- Tests negative values: "-45.67", "-45,67"
- Tests scientific notation: "2e8", "4.21875e-5"
- Tests error handling: invalid input

### Usage Examples

**User Input (supports both formats):**
```
1.23    ✓ Accepted
1,23    ✓ Accepted
0.0225  ✓ Accepted
0,0225  ✓ Accepted
2e8     ✓ Accepted (scientific notation)
```

**Saved Data (always uses dot):**
```
Member,Area,Iz,E
1,0.0225,4.21875e-5,2e8
```

## Benefits
1. **International Compatibility**: Users with different locale settings (e.g., European comma-based formats) can now enter decimal values naturally
2. **Robust Parsing**: Multiple fallback strategies prevent parsing failures
3. **Consistent Storage**: InvariantCulture formatting ensures saved files work across all locales
4. **User-Friendly**: Clear error messages when input cannot be parsed
5. **Minimal Changes**: Maintains existing functionality while adding flexibility

## Migration Notes
- All existing data files remain compatible (they use dot separators)
- Users can now input values using either dot or comma
- No changes required to existing projects or data files

## Future Enhancements
Potential areas for further improvement:
- Add unit tests for InputParsingHelpers
- Consider adding thousand separators support
- Add validation ranges to helper methods
- Localized error messages
