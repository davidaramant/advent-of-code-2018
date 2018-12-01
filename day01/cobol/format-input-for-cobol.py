# Formats the input as fixed-width values
with open('input.txt', 'r') as original_input:
    with open('input-cobol.txt', 'w') as cobol_output:
        for line in original_input:
            change = int(line)
            cobol_output.write('{:+07d}\n'.format(change))