select 
c.*,
COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') AS IS_IDENTITY,
i.is_primary_key AS IS_PRIMARY_KEY
from INFORMATION_SCHEMA.COLUMNS c
LEFT OUTER JOIN sys.indexes i
	ON i.object_id = COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'ColumnId')
WHERE TABLE_NAME = 'Account'

SELECT i.name, i.is_primary_key 
FROM
	sys.indexes i
INNER JOIN
    sys.tables t
        ON i.object_id = t.object_id
INNER JOIN
	sys.index_columns ic
		ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN
	sys.columns c
		ON ic.object_id = c.object_id AND c.column_id = COLUMNPROPERTY(OBJECT_ID('User'), 'UserID', 'ColumnId')