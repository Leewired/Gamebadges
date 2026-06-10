Vector3 = {x = 0, y = 0, z = 0}
Vector3.__index = Vector3

function Vector3:new (x, y, z)
	local self = setmetatable({}, Vector3)
	self.x = x or 0
	self.y = y or 0
	self.z = z or 0
	self.length = math.sqrt(self.x^2 + self.y^2 + self.z^2)
	print("The length of the vector is:", self.length)
	return self
end


function Vector3:PrintVector3Length()
	print("For the second time: the length of the vector is: ", self.length)
	s = "For the third time: the length of the vector is: ", self.length
	return s
end
