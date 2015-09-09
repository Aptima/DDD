#!/
import sys
if len(sys.argv) != 3:
	print "USAGE: " + sys.argv[0] + " [template file] [data csv]"
	exit()

templateFileName = sys.argv[1]
dataFileName = sys.argv[2]

templateFile = open(templateFileName,'r')
dataFile = open(dataFileName,'r')
templateText = templateFile.read()
templateFile.close()

lines = dataFile.readlines()

dataTagLine = lines[0]
lines.remove(lines[0])

dataTagLine = dataTagLine.strip()
dataTags = dataTagLine.split(',')
#data = []

#print templateText

for line in lines:
	d = line.strip().split(',')
	assert(len(d) == len(dataTags))
	#rec = {}
	newText = templateText
	for i in range(len(dataTags)):
		#rec[dataTags[i]] = d[i]
		#print rec
		#data.append(rec)
		#newText = templateText
		#for tag in dataTags:
		#	newText = newText.replace(tag,rec[tag])
		#print newText
		newText = newText.replace(dataTags[i],d[i])
	print newText
#print data
