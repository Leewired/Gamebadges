-- GameBadges example

print("Startup script printed, Hi!")

-- Example syntax

--[[
function GiveValue()
	return 10
end

v = GiveValue()
print(string.format("Value is %s", v))


function AddValues(a, b)
	return a + b
end


a = 10
b = 50
v = AddValues(a, b)
print(string.format("%s + %s equals %s", a, b, v))


function HelloWorld()
	print("Hello World!")
end

HelloWorld();


i = 0
for i = 1, 10, 1 do
	print(i)
end


while (i < 10) do
	print (i)
	i = i + 1
end


a = {}
for i = 0, 9 do
	a[i] = i
end

for v in pairs(a) do
	print(v)
end

--]]