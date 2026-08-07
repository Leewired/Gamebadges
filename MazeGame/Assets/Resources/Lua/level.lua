
function OnDialogue(lineId)
	local s = GetDialogueLine(1)
	SetIntroText(s)
	local p = GetDialogueLine(2)
	SetPauseText(p)
	local v = GetDialogueLine(3)
	SetEndText(v)
	local go = GetDialogueLine(4)
	SetEndText(go)
end
